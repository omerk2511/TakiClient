using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taki
{
    public class Client
    {
        private byte[] serverIp = { 104, 156, 225, 184 };
        private short serverPort = 8080;
        Socket gameSocket;
        public string jwt { get; set; }

        private List<dynamic> jsonList = new List<dynamic>();

        public Client()
        {

            try
            {
                IPAddress ipAddress = new IPAddress(serverIp);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, serverPort);

                this.gameSocket = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);
                gameSocket.Connect(remoteEP);
            }
            catch (SocketException e)
            {
                MessageBox.Show("Server is not responding, please make sure the server is running or try again later.");
                Environment.Exit(0);
            }
        }

        public void SendJSON(object jsonObj)
        {
            try
            {
                string json = JsonConvert.SerializeObject(jsonObj);
                Console.WriteLine("send " + json);
                this.gameSocket.Send(Encoding.ASCII.GetBytes(json));
            }
            catch (SocketException e)
            {
                MessageBox.Show("Server is not responding, please make sure the server is running or try again later.");
                Environment.Exit(0);
            }
        }

        public object RecvJSON(bool blocking)
        {
            try
            {
                if (jsonList.Count == 0)
                {
                    byte[] stringToParse = new byte[4096];
                    this.gameSocket.Blocking = blocking;
                    int bytesRecieved;
                    try
                    {
                        bytesRecieved = this.gameSocket.Receive(stringToParse);
                        if (bytesRecieved == 0)
                        {
                            return null;
                        }
                    }
                    catch (SocketException e)
                    {
                        return null;
                    }
                    Stack<int> jsonCurlyBracesStack = new Stack<int>();
                    bool quotationMarkFlag = false;
                    for (int i = 0; i < bytesRecieved; i++)
                    {
                        if (stringToParse[i] == 0)
                        {
                            break;
                        }
                        int firstJsonIndex = 0;
                        int lastJsonIndex = 0;
                        if ((char)(stringToParse[i]) == '{' && !quotationMarkFlag)
                        {
                            jsonCurlyBracesStack.Push(i);
                        }
                        else if ((char)(stringToParse[i]) == '}' && !quotationMarkFlag)
                        {
                            firstJsonIndex = jsonCurlyBracesStack.Pop();
                            lastJsonIndex = i;
                        }
                        else if ((char)(stringToParse[i]) == '\"' && (char)(stringToParse[i - 1]) != '\\')
                        {
                            quotationMarkFlag = !quotationMarkFlag;
                        }

                        if (jsonCurlyBracesStack.Count == 0)
                        {
                            try
                            {
                                byte[] jsonStr = stringToParse.Skip(firstJsonIndex).Take(lastJsonIndex - firstJsonIndex + 1).ToArray();
                                jsonList.Add(JsonConvert.DeserializeObject(Encoding.ASCII.GetString(jsonStr)));
                            }
                            catch (Newtonsoft.Json.JsonException e)
                            {
                                throw new TakiServerException("Cannot convert server data to json object." + e.Message);
                            }
                        }
                    }
                }

                if (jsonList.Count != 0)
                {
                    dynamic jsonObj = jsonList[0];
                    jsonList.RemoveAt(0);
                    Console.WriteLine("Recieved: " + jsonObj.ToString());

                    return jsonObj;
                }
                else
                {
                    throw new TakiServerException("Cannot convert server data to json object");
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show("Server is not responding, please make sure the server is running or try again later.");
                Environment.Exit(0);
                return null;
            }
            catch (TakiServerException e)
            {
                MessageBox.Show(e.Message);
                Environment.Exit(0);
                return null;
            }
        }
    }
}

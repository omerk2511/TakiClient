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
            }
        }

        public void SendJSON(object jsonObj)
        {
            string json = JsonConvert.SerializeObject(jsonObj);
            this.gameSocket.Send(Encoding.ASCII.GetBytes(json));
        }

        public object RecvJSON() 
        {
            if (jsonList.Count == 0)
            {
                byte[] stringToParse = new byte[4096];
                int bytesRecieved = this.gameSocket.Receive(stringToParse);

                Stack<int> jsonStack = new Stack<int>();

                for (int i = 0; i < bytesRecieved; i++)
                {
                    if(stringToParse[i] == 0)
                    {
                        break;
                    }
                    int firstJsonIndex = 0;
                    int lastJsonIndex = 0;
                    if ((char)(stringToParse[i]) == '{')
                    {
                        jsonStack.Push(i);
                    }
                    else if ((char)(stringToParse[i]) == '}')
                    {
                        firstJsonIndex = jsonStack.Pop();
                        lastJsonIndex = i;
                    }

                    if (jsonStack.Count == 0)
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

            if(jsonList.Count != 0)
            {
                dynamic jsonObj = jsonList[0];
                jsonList.RemoveAt(0);
                Console.WriteLine(jsonObj);
                return jsonObj;
            }
            else
            {
                throw new TakiServerException("Cannot convert server data to json object");
            }
        }

    }
}

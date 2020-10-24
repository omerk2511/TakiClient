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
                MessageBox.Show("Server is not reponding, please make sure the server is running or try again later.");
            }
        }

        public void SendJSON(object jsonObj)
        {
            string json = JsonConvert.SerializeObject(jsonObj);
            Console.WriteLine(json);
            this.gameSocket.Send(Encoding.ASCII.GetBytes(json));
        }

        public object RecvJSON() 
        {
            byte[] jsonStr = new byte[1024];
            this.gameSocket.Receive(jsonStr);

            dynamic jsonObj = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(jsonStr));
            return jsonObj;
        }

    }
}

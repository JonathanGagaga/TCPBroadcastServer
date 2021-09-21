using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace JonathanServer
{
    public class ServerControl
    {

        private Socket serverSocket;
        
        private List<Socket> clientList;

        public ServerControl()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientList = new List<Socket>();
        }

        public void Start()
        {
            
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 22));
            serverSocket.Listen(10);
            Console.WriteLine("Server Starts Successfully");

            
            Thread threadAccept = new Thread(Accept);
            threadAccept.IsBackground = true;
            threadAccept.Start();
        }

        private void Accept()
        {
            
            Socket client = serverSocket.Accept();
            IPEndPoint point = client.RemoteEndPoint as IPEndPoint;
            Console.WriteLine(point.Address + "[" + point.Port + "] connected successfully");
            clientList.Add(client);

           
            Thread threadReceive = new Thread(Receive);
            threadReceive.IsBackground = true;
            threadReceive.Start(client);

            Accept();
        }
        private void Receive(object obj)
        {
        
            Socket client = obj as Socket;

            IPEndPoint point = client.RemoteEndPoint as IPEndPoint;

            try
            {
                byte[] msg = new byte[1024];

                int msgLen = client.Receive(msg);

                string msgStr = point.Address + "[" + point.Port + "]:" + Encoding.Default.GetString(msg, 0, msgLen);
                Console.WriteLine(msgStr);

                Broadcast(client, msgStr);

                Receive(client);
            }
            catch
            {
                Console.WriteLine(point.Address + "[" + point.Port + "] is disconnected");
                clientList.Remove(client);
            }

        }
        private void Broadcast(Socket clientOther, string msg)
        {
            foreach(var client in clientList)
            {
                if(client == clientOther)
                {

                }
                else
                {
                    client.Send(Encoding.Default.GetBytes(msg));
                }
            }
        }
    }
}

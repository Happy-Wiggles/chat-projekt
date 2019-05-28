using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Server
{
    class Program
    {
        public static bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
        static void Main(string[] args)
        {
            byte[] sendMsg = new byte[4096];
            sendMsg = Encoding.ASCII.GetBytes("Nachricht erhalten!");
            byte[] receivedMsg = new byte[4096];
            String myIp = "";

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            

            foreach (IPAddress ip in ipHostInfo.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ip;
                    myIp = ip.ToString();
                }
            }
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1600);
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


            listener.Bind(localEndPoint);

            listener.Listen(10);

            Console.WriteLine("Waiting for a connection...");
            
            Socket handler = listener.Accept();
            while (true)
            {

                if (SocketConnected(handler))
                {
                    Console.WriteLine("verbunden");
                    handler.Receive(receivedMsg);

                    
                    int i = receivedMsg.Length - 1;
                    while (receivedMsg[i] == 0)
                        --i;

                    byte[] bar = new byte[i + 1];
                    for (int x = 0; x < i + 1; x++)
                    {
                        bar[x] = receivedMsg[x];
                    }

                    Console.WriteLine(Encoding.ASCII.GetString(bar));
                    handler.Send(sendMsg);
                }
                
            }
        }
    }
}

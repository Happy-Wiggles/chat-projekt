using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Server
{
    class Program
    {
        private static byte[] buffer = new byte[4096];
        private static List<Socket> Sockets = new List<Socket>();
        private static Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            setupServer();
            Console.ReadLine();
        }
        private static void setupServer()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            foreach (IPAddress ip in ipHostInfo.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ip;
                }
            }
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1600);
            listener.Bind(localEndPoint);
            listener.Listen(10);
            Console.WriteLine("Waiting for a connection...");
            listener.BeginAccept(new AsyncCallback(acceptCallback), null);
        }
        private static void acceptCallback(IAsyncResult AR)
        {
            Socket handler = listener.EndAccept(AR);
            Sockets.Add(handler);
            handler.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveCallback), handler);
            listener.BeginAccept(new AsyncCallback(acceptCallback), null);
        }
        private static void receiveCallback(IAsyncResult AR)
        {
            Socket handler = (Socket)AR.AsyncState;
            int received = handler.EndReceive(AR);
            byte[] dataBuff = new byte[received];
            Array.Copy(buffer, dataBuff, received);
            string text = Encoding.ASCII.GetString(dataBuff);
            if (!(text == ""))
            {
                Console.WriteLine(text);
            }
            handler.BeginSend(dataBuff, 0, dataBuff.Length, SocketFlags.None, new AsyncCallback(sendCallback), handler);
            handler.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveCallback), handler);
        }
        private static void sendCallback(IAsyncResult AR)
        {
            Socket handler = (Socket)AR.AsyncState;
            handler.EndSend(AR);
        }
    }
}

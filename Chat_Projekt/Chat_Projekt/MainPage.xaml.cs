using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Chat_Projekt
{
    
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        static IPAddress ipAddress = IPAddress.Parse("192.168.2.107"); //192.168.2.107 //79.197.19.253
        static IPEndPoint remoteEP = new IPEndPoint(ipAddress, 1600);
        static Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
      
        
        public MainPage()
        {
            InitializeComponent();
            loopConnect();
            Thread receiverThread = new Thread(()=> loopReceive());
            receiverThread.Start();
        }

        private void loopReceive()
        {
            int scrollToPixel = 0;
            
            while (true)
            {
                byte[] receivedMsg = new byte[4096];
                
                client.Receive(receivedMsg);

                Device.BeginInvokeOnMainThread(() =>
                {
                    Ausgabe.Text = Ausgabe.Text + "\n" + Encoding.ASCII.GetString(receivedMsg);
                    scrollBox.ScrollToAsync(0,scrollToPixel,true);
                    scrollToPixel = scrollToPixel + 100;
                });
            }
        }

        private static void loopConnect()
        {
            while (!client.Connected)
            {
                client.Connect(remoteEP);
            }
        }
        private void SendenButton_Clicked(object sender, EventArgs e)
        {
                byte[] sendMsg = new byte[4096];
                string message = "";
                message = Eingabe.Text;

                if (message != null)
                {
                    sendMsg = Encoding.ASCII.GetBytes(Eingabe.Text);
                    if (client.Connected)
                    {
                        client.Send(sendMsg);
                    }
                    else
                    {
                        client.Close();
                    }
                }
            }
        }
    }


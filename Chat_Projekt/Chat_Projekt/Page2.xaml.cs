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
    public partial class Page2 : ContentPage
    {
        static IPAddress ipAddress ; //192.168.2.107 //79.197.19.253
        static IPEndPoint remoteEP ;
        Socket client ;
        string name = "";
        

        public Page2(string name, IPAddress ip)
        {
            
            InitializeComponent();
            ipAddress = ip;
            remoteEP = new IPEndPoint(ipAddress, 1600);
            client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            loopConnect();
            Thread receiverThread = new Thread(()=> loopReceive());
            receiverThread.Start();
            this.name = name;
            username.Text = name;
        }
       
        private void loopReceive()
        {
            int scrollToPixel = 0;
            
            while (true)
            {

                byte[] receivedMsg = new byte[4096];

                try
                {

                    client.Receive(receivedMsg);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Ausgabe.Text = Ausgabe.Text + "\n" + Encoding.ASCII.GetString(receivedMsg);
                        scrollBox.ScrollToAsync(0, scrollToPixel, true);
                        scrollToPixel = scrollToPixel + 100;
                    });
                }
                catch (Exception e)
                {

                }

            }
        }
       
        private void loopConnect()
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
                    sendMsg = Encoding.ASCII.GetBytes(name + ": " + Eingabe.Text);
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


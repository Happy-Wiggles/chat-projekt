﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Net;
using System.Net.Sockets;

namespace Chat_Projekt
{
    
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        static IPAddress ipAddress = IPAddress.Parse("192.168.0.5");
        static IPEndPoint remoteEP = new IPEndPoint(ipAddress, 1600);
        byte[] sendMsg = new byte[4096];
        byte[] receivedMsg = new byte[4096];

        static Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        public MainPage()
        {
            InitializeComponent();
            client.Connect(remoteEP);
            Ausgabe.Text = "Socket connected to " + client.RemoteEndPoint.ToString();
        }

        private void SendenButton_Clicked(object sender, EventArgs e)
        {

            sendMsg = Encoding.ASCII.GetBytes(Eingabe.Text);
            
            client.Send(sendMsg);
            client.Receive(receivedMsg);
            Ausgabe.Text = Encoding.ASCII.GetString(receivedMsg);
        }
    }
}

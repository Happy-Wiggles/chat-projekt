using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Net;
using System.Net.Sockets;

namespace Chat_Projekt
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public string name = "";
        public Page1()
        {
            
            InitializeComponent();
            
        }

        private async void BestaetigenButton_Clicked(object sender, EventArgs e)
        {
            name = Eingabe.Text;
            await Navigation.PushAsync(new Page2(name));
        }
    }
}
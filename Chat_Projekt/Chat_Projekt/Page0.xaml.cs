using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chat_Projekt
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page0 : ContentPage
    {
        public Page0()
        {
            InitializeComponent();
        }
        private async void StartButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Page1());
        }
    }
}
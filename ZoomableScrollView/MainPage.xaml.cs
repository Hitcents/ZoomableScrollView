using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ZoomableScrollView
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void BoxScrollPage(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new BoxScrollPage());
        }

        private async void ExampleScrollPage(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ExampleScrollPage());
        }

        private async void PanGesturePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PanGesturePage());
        }
    }
}


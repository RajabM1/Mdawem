using MdawemApp.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp
{
    public partial class App : Application
    {
      
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new SplashScreen());
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        private async static void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            bool isConnected = e.NetworkAccess == NetworkAccess.Internet;
            if (isConnected)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Internet connection is available.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Internet connection is not available. Please check your internet connection and try again.", "OK");

                var navigationPage = Application.Current.MainPage as NavigationPage;
                var currentPage = navigationPage?.CurrentPage as ContentPage;
                // Push the current page onto the stack only if it is still the active page
                await currentPage.Navigation.PushAsync(new LogInPage(), true);

            }
        }


    }
}

using MdawemApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            InternetTest();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async static void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = await client.OpenReadTaskAsync("http://www.google.com")) ;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Internet connection is not available.", "OK");

                var navigationPage = Application.Current.MainPage as NavigationPage;
                var currentPage = navigationPage?.CurrentPage as ContentPage;
                var navigationStack = currentPage?.Navigation.NavigationStack;

                if (navigationStack?.Count > 0)
                {
                    var previousPage = navigationStack.LastOrDefault();
                    if (previousPage != null)
                    {
                        await currentPage.Navigation.PushAsync(new InternetConnection(), true);
                    }
                }

                else
                {
                    var flyoutPage = Application.Current.MainPage as NavigationPage;
                    var currentPageNew = flyoutPage?.CurrentPage as FlyoutPage;
                    var flyoutStack = currentPageNew?.Navigation.NavigationStack;

                    if (flyoutStack?.Count > 0)
                    {
                        var previousPage = flyoutStack.LastOrDefault();
                        if (previousPage != null)
                        {
                            await currentPageNew.Navigation.PushAsync(new InternetConnection(), true);
                        }
                    }
                }
            }
        }

        public async void InternetTest()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = await client.OpenReadTaskAsync("http://www.google.com")) ;
            }
            catch
            {
                await Task.Delay(3500);

                await Application.Current.MainPage.DisplayAlert("Error", "Internet connection is not available.", "OK");
                var navigationPage = Application.Current.MainPage as NavigationPage;
                var currentPage = navigationPage?.CurrentPage as ContentPage;
                var navigationStack = currentPage?.Navigation.NavigationStack;

                if (navigationStack?.Count > 0)
                {
                    var previousPage = navigationStack.LastOrDefault();
                    if (previousPage != null)
                    {
                        await currentPage.Navigation.PushAsync(new InternetConnection(), true);
                    }
                }
                
            }
        }

    }
}

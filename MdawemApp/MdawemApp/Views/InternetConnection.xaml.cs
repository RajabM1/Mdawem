using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InternetConnection : ContentPage
    {
        public InternetConnection()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

       private void TryAgain_Clicked(object sender, EventArgs e)
        {
            _ = CheckInternetConnectionAsync();
        }

        public async Task<bool> CheckInternetConnectionAsync()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = await client.OpenReadTaskAsync("http://www.google.com"))
                {
                    var navigationPage = Application.Current.MainPage as NavigationPage;
                    var currentPage = navigationPage?.CurrentPage as ContentPage;
                    var navigationStack = currentPage?.Navigation.NavigationStack;

                    if (navigationStack?.Count > 0)
                    {
                        var errorMessagePage = navigationStack.LastOrDefault();
                        if (errorMessagePage != null)
                        {
                            currentPage.Navigation.RemovePage(errorMessagePage);
                        }
                    }

                    if (navigationStack?.Count > 0)
                    {
                        var previousPage = navigationStack.LastOrDefault();
                        if (previousPage != null)
                        {
                            await currentPage.Navigation.PushAsync(previousPage, true);
                        }
                    }
                    else
                    {
                        await currentPage.Navigation.PopAsync();
                    }
                    
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
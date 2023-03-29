using Firebase.Auth;
using MdawemApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogInPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        public LogInPage()
        {
            InitializeComponent();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (Application.Current.Properties.ContainsKey("emailtxt") && Application.Current.Properties.ContainsKey("passwordtxt"))
            {
                string email = (string)Application.Current.Properties["emailtxt"];
                string pass = (string)Application.Current.Properties["passwordtxt"];
                string token = await firebaseHelper.Login(email, pass);
                if (!string.IsNullOrEmpty(token))
                {
                    var tabbedPage = new TabbedPage1();
                    tabbedPage.CurrentPage = tabbedPage.Children[0]; // Set the second page as the current page
                    //MainPage = tabbedPage;
                    await Navigation.PushAsync(tabbedPage);
                }
            }
        }
        private async void btnlogin_Clicked(object sender, EventArgs e)
        {

            try
            {

                string email = emailtxt.Text;
                string password = passwordtxt.Text;
                bool rememberMe = RememberMeCheckBox.IsChecked;
                if (rememberMe)
                {
                    Application.Current.Properties["emailtxt"] = email;
                    Application.Current.Properties["passwordtxt"] = password;
                }
                else
                {
                    Application.Current.Properties.Remove("emailtxt");
                    Application.Current.Properties.Remove("passwordtxt");
                }
                if (string.IsNullOrEmpty(email))
                {

                    await DisplayAlert("Warning  ", "enter your email", "OK");
                    return;

                }
                if (string.IsNullOrEmpty(password))
                {

                    await DisplayAlert("Warning  ", "enter your password", "OK");
                    return;

                }

                string token = await firebaseHelper.Login(email, password);
                if (!string.IsNullOrEmpty(token))
                {
                    var tabbedPage = new TabbedPage1();
                    tabbedPage.CurrentPage = tabbedPage.Children[0]; // Set the second page as the current page
                    //MainPage = tabbedPage;
                    await Navigation.PushAsync(tabbedPage);
                }
                else
                {
                    await DisplayAlert("Login ", "Login failled", "OK");
                }

            }
            catch (Exception excption)
            {
                if (excption.Message.Contains("EMAIL_NOT_FOUND"))
                {
                    await DisplayAlert("Unauthorized  ", "Login failled email or password not found ", "OK");

                }
                else if (excption.Message.Contains("INVALID_PASSWORD"))
                {
                    await DisplayAlert("Unauthorized  ", "Login failled email or password is not found ", "OK");

                }
                else
                {
                    await DisplayAlert("Error ", excption.Message, "OK");
                }
            }

        }
    }

}

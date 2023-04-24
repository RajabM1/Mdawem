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
                activityIndicator.IsRunning = true;
                activityIndicator.IsVisible = true;

                string email = (string)Application.Current.Properties["emailtxt"];
                string pass = (string)Application.Current.Properties["passwordtxt"];
                string token = await firebaseHelper.Login(email, pass);
                if (!string.IsNullOrEmpty(token))
                {
                    activityIndicator.IsRunning = false;
                    activityIndicator.IsVisible = false;

                    await Navigation.PushAsync(new HomePage());
                }
            }
        }
        private async void btnlogin_Clicked(object sender, EventArgs e)
        {

            try
            {
                LogInButton.IsEnabled= false;
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
                    LogInButton.IsEnabled = true;
                    return;

                }
                if (string.IsNullOrEmpty(password))
                {

                    await DisplayAlert("Warning  ", "enter your password", "OK");
                    LogInButton.IsEnabled = true;
                    return;

                }
                activityIndicator.IsRunning = true;
                activityIndicator.IsVisible = true;

                string token = await firebaseHelper.Login(email, password);
                if (!string.IsNullOrEmpty(token))
                {
                    activityIndicator.IsRunning = false;
                    activityIndicator.IsVisible = false;
                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    activityIndicator.IsRunning = false;
                    activityIndicator.IsVisible = false;

                    await DisplayAlert("Login ", "Login failled", "OK");
                }
                LogInButton.IsEnabled = true;

            }
            catch (Exception excption)
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;

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
                LogInButton.IsEnabled = true;
            }

        }

        private async void ForgetPassword(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPassword());
        }
    }

}

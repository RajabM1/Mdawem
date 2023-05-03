using Firebase.Auth;
using MdawemApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            NavigationPage.SetHasNavigationBar(this, false);

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

                    await Navigation.PushAsync(new FlyoutPage1());
                }
            }
        }
        private async void btnlogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                LogInButton.IsEnabled = false;
                string email = emailtxt.Text.Trim(); // remove any extra spacing from email
                string password = passwordtxt.Text;
                bool rememberMe = RememberMeCheckBox.IsChecked;
                Application.Current.Properties["emailtxt"] = email;

                if (rememberMe)
                {
                    Application.Current.Properties["passwordtxt"] = password;
                }
                else
                {
                    Application.Current.Properties.Remove("passwordtxt");
                }

                if (string.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Warning", "Please enter your email.", "OK");
                    LogInButton.IsEnabled = true;
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Warning", "Please enter your password.", "OK");
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
                    await Navigation.PushAsync(new FlyoutPage1());
                    passwordtxt.Text = "";
                    emailtxt.Text = "";
                }
                else
                {
                    activityIndicator.IsRunning = false;
                    activityIndicator.IsVisible = false;
                    await DisplayAlert("Login Failed", "Invalid email and password. Please try again.", "OK");
                }

                LogInButton.IsEnabled = true;
            }
            catch (HttpRequestException)
            {
                await DisplayAlert("Error", "An error occurred while logging in. Please check your internet connection and try again.", "OK");
                LogInButton.IsEnabled = true;
            }
            catch (Exception exception)
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;

                if (exception.Message.Contains("EMAIL_NOT_FOUND"))
                {
                    await DisplayAlert("Unauthorized", "Login failed. Email or password not found.", "OK");
                }
                else if (exception.Message.Contains("INVALID_PASSWORD"))
                {
                    await DisplayAlert("Unauthorized", "Login failed. Email or password is incorrect.", "OK");
                }

                else
                {
                    await DisplayAlert("Error", exception.Message, "OK");
                }

                LogInButton.IsEnabled = true;
            }
        }

        private async void ForgetPassword(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPassword());
        }

        private bool isPasswordVisible;
        private void OnEyeIconTapped(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            passwordtxt.IsPassword = !passwordtxt.IsPassword;
            hideEyeImage.IsVisible = !isPasswordVisible;
            openEyeImage.IsVisible = isPasswordVisible;
        }
    }

}

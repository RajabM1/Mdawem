using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MdawemApp.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {

        public Validation validate;

        public FirebaseHelper fireBase;
        public ForgotPassword()
        {
            InitializeComponent();
            validate = new Validation();
            fireBase = new FirebaseHelper();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!validate.IsValidEmail(emailEntry.Text))
            {
                emailErrorMessage.Text= "Invalid Email";
            }
            else
            {
                emailErrorMessage.Text = " ";
            }
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {

            if (string.IsNullOrEmpty(emailEntry.Text) && emailErrorMessage.Text == " ")
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Enter your email", "OK");
            }
            else if (emailErrorMessage.Text == "Invalid Email")
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid email", "OK");
            }
            else
            {
                bool isSuccessful = await fireBase.ResetPassword(emailEntry.Text);
                if (isSuccessful)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Please Check your E-Mail to update your password", "OK");

                    await Navigation.PopAsync();
                }
               
            }
        }

    }
}
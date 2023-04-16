using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MdawemApp.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
                emailErrorMessage.IsVisible = true;
            }
            else
            {
                emailErrorMessage.IsVisible = false;
            }
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(emailEntry.Text) && emailErrorMessage.IsVisible == false)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Enter your email", "OK");
            }
            else if (emailErrorMessage.IsVisible)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid email", "OK");
            }
            else
            {
                bool isSuccessful = await fireBase.ResetPassword(emailEntry.Text);
                if (isSuccessful)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Invalid email", "OK");
                }
            }
        }

    }
}
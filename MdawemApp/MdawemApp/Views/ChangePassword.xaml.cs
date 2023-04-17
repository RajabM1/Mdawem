using MdawemApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePassword : ContentPage
    {
        public Validation validate;

        public FirebaseHelper fireBase;
        public ChangePassword()
        {
            InitializeComponent();
            validate = new Validation();
            fireBase = new FirebaseHelper();

        }

        private void passTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!validate.IsValidPassword(passTxt.Text))
            {
                passErrorMsg.IsVisible = true;
            }
            else
            {
                passErrorMsg.IsVisible = false;
            }
        }

        private async void ResetPassButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string password = passTxt.Text;
                string confirmPass = confPassTxt.Text;

                if (string.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Change Password", "Please enter the password", "OK");
                    return;
                }
                if (passErrorMsg.IsVisible == true)
                {
                    await DisplayAlert("Change Password", "Please enter a valid password", "OK");
                    return;
                }
                if (string.IsNullOrEmpty(confirmPass))
                {
                    await DisplayAlert("Change Password", "Please enter confirm password", "OK");
                    return;
                }
                if (password != confirmPass)
                {
                    await DisplayAlert("Change Password", "Confirm password not match.", "OK");
                    return;
                }
                string token = Preferences.Get("token", String.Empty);
                bool isChanged = await fireBase.ChangePassword(token, password);
                if (isChanged)
                {
                    await DisplayAlert("Change Password", "Password has been changed.", "OK");
                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    await DisplayAlert("Change Password", "Password change failed ", "OK");
                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
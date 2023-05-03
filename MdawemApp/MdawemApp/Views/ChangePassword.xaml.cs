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
                passErrorMsg.Text = "Invalid Password";
            }
            else
            {
                passErrorMsg.Text = "          ";
            }
        }

        private async void ResetPassButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string currentPassword = currentPassTxt.Text;
                string newPassword = passTxt.Text;
                string confirmPass = confPassTxt.Text;

                if (string.IsNullOrEmpty(currentPassword)|| string.IsNullOrEmpty(newPassword)|| string.IsNullOrEmpty(confirmPass))
                {
                    await DisplayAlert("Change Password", "Please fill all input", "OK");
                    return;
                }
               
                if (passErrorMsg.Text == "Invalid Password")
                {
                    await DisplayAlert("Change Password", "Please enter a valid password", "OK");
                    return;
                }
                
                if (newPassword != confirmPass)
                {
                    await DisplayAlert("Change Password", "Confirm password not match.", "OK");
                    return;
                }
                string token = Preferences.Get("token", String.Empty);

                bool isCurrentPasswordCorrect = await fireBase.VerifyPassword(token, currentPassword);
                if (!isCurrentPasswordCorrect)
                {
                    await DisplayAlert("Change Password", "The current password is incorrect", "OK");
                    return;
                }
                bool isChanged = await fireBase.ChangePassword(token, newPassword);
                if (isChanged)
                {
                    await DisplayAlert("Change Password", "Password has been changed.", "OK");
                    await Navigation.PopAsync();
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

        private void confPassTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newPassword = passTxt.Text;
            string confirmPass = confPassTxt.Text;
            if (newPassword != confirmPass && validate.IsValidPassword(passTxt.Text)) 
            {
                confirmPassErrorMsg.Text = "Password does not match";
            }
            else if(newPassword != confirmPass && !validate.IsValidPassword(passTxt.Text))
            {
                confirmPassErrorMsg.Text = "Invalid Password";

            }
            else
            {
                confirmPassErrorMsg.Text = "          ";
            }

            
        }
    }
}
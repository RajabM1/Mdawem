using Firebase.Database;
using Firebase.Database.Query;
using MdawemApp.Helper;
using MdawemApp.Models;
using MdawemApp.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        FirebaseHelper FirebaseHelper;
        MediaFile mediaFile;
        public ProfilePage()
        {
            InitializeComponent();
            FirebaseHelper = new FirebaseHelper();
            display();
            // var employee = firebaseHelper.GetAll();
            // BindingContext = new ProfileViewModel { Employees = employee };
        }

        public async void display()
        {
            var info = await FirebaseHelper.GetInfo();
            foreach (Employee item in info)
            {
                Email.Text = item.Email;
                FirstName.Text = item.FirstName;
                address.Text = item.HomeAddress;
                phone.Text = item.PhoneNumber;
                birthday.Text = item.DateOfBirth.ToString();
            }
        }

        private async void Lougoutbtn(object sender, EventArgs e)
        {
            try
            {
                FirebaseHelper.SignOut();
                await Application.Current.MainPage.Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception here, for example:
                Debug.WriteLine($"Error in Lougoutbtn: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred. Please try again later.", "OK");
            }
        }


        private async void update(object sender, EventArgs e)
        {
            Employee updatedEmployee = new Employee
            {
                FirstName = FirstName.Text,
                Email = Email.Text,
                PhoneNumber = phone.Text,
                HomeAddress = address.Text,
                DateOfBirth = DateTime.Now,
            };
            await FirebaseHelper.UpdateEmployee(updatedEmployee);
        }
      /* private async void TapGestureRecognizer_Tapped (object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                mediaFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (mediaFile == null)
                {
                    return;
                }
            }catch (Exception ex) { }
        }*/
    }
}
using Firebase.Database;
using Firebase.Database.Query;
using MdawemApp.Helper;
using MdawemApp.Models;
using MdawemApp.ViewModels;
using System;
using System.Collections.Generic;
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
        public ProfilePage()
        {
            InitializeComponent();
            FirebaseHelper = new FirebaseHelper();
            //display();
            // var employee = firebaseHelper.GetAll();
            // BindingContext = new ProfileViewModel { Employees = employee };
        }

        /*public async void display()
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
        }*/

        private async void Lougoutbtn(object sender, EventArgs e)
        {
            FirebaseHelper.SignOut();
            await Navigation.PushAsync(new LogInPage());
        }


        private async void update(object sender, EventArgs e)
        {
            Employee updatedEmployee = new Employee
            {
                FirstName = FirstName.Text,
                Email = Email.Text,
                PhoneNumber = phone.Text,
               /* HomeAddress = address.Text,
                DateOfBirth = DateTime.Now,*/
            };
            await FirebaseHelper.UpdateEmployee(updatedEmployee);
        }
    }
}
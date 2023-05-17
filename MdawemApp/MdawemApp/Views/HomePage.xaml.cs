using MdawemApp.Helper;
using MdawemApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        FirebaseHelper firebaseHelper;
        List<Attendance> Attends = new List<Attendance>();
        List<VactionRequestModel> leave = new List<VactionRequestModel>();

        public HomePage()
        {
            InitializeComponent();
            firebaseHelper = new FirebaseHelper();
            SetInformations();
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasBackButton(this, false);
            var infos = await firebaseHelper.GetUserInformation();
            if (!infos.EmploymentStatus)
            {
                this.ToolbarItems.Remove(NotificationTool);
            }
        }

        async void SetInformations()
        {
            DateTime dateTime = DateTime.Now;
            string date = dateTime.ToString("dd MMMM yyyy");
            TodayDate.Text = date;
           
            var UID = Application.Current.Properties["UID"].ToString();
            string CurrentYear = dateTime.Year.ToString();
            string monthString = dateTime.Month.ToString("D2");

            var infos = await firebaseHelper.GetUserInformation();
            string name = $"{infos.FirstName}, {infos.LastName}";
            Name.Text = name;

            Attends = await firebaseHelper.GetAttendance(UID, CurrentYear, monthString);
            if (Attends == null || Attends.Count == 0)
            {
                TimeInDate.Text = date;
                TimeOutDate.Text = date;
                TimeIn.Text = "Unshifted yet";
                TimeOut.Text = "Unshifted yet";
            }
            else
            {
                var lastAttendance = Attends.LastOrDefault();
                TimeInDate.Text = lastAttendance.Date;
                TimeOutDate.Text = lastAttendance.Date;
                TimeIn.Text = lastAttendance.TimeIn;
                if (lastAttendance.TimeOut != "")
                {
                    TimeOut.Text = lastAttendance.TimeOut;
                }
                else
                {
                    TimeOut.Text = "Unfinished shift";
                }
            }
            var leave = await firebaseHelper.GetLeaves(UID, CurrentYear, null);
            if (leave == null|| leave.Count==0)
            {
                Dateofrequest.Text = date;
                Status.Text = "There isn't any leave";
            }
            else
            {
                var lastLeave = leave.LastOrDefault();
                Dateofrequest.Text= lastLeave.Dateofrequest;
                Status.Text = lastLeave.Status;
            }

        }

        private async void CheckIn_Clicked(object sender, EventArgs e)
        {
          
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (status != PermissionStatus.Granted)
                    {
                        throw new Exception("Location permission denied.");
                    }
                }
                var location = await Geolocation.GetLocationAsync();
                if (location != null)
                {
                    var uid = Application.Current.Properties["UID"].ToString();

                    bool CheckStatus = await firebaseHelper.CheckIn(uid, location.Latitude, location.Longitude);
                    if (CheckStatus)
                    {
                        await DisplayAlert("Success", "Check-in Is Done", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Alert", "You are already checked in, please check out first", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Unable to get location", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }
        private async void CheckOut_Clicked(object sender, EventArgs e)
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (status != PermissionStatus.Granted)
                    {
                        throw new Exception("Location permission denied.");
                    }
                }

                var location = await Geolocation.GetLocationAsync();
                var uid = Application.Current.Properties["UID"].ToString();
                bool CheckStatus = await firebaseHelper.CheckOut(uid);
                if (CheckStatus) 
                {
                    await DisplayAlert("Success", "Check-out Is Done", "OK");
                }
                else
                {
                    await DisplayAlert("Alert", "You are already checked out, please check in first", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
		}

       
        private async void RequestOnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RequestForVacation());
        }

        /*private async void OnProfileIconClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }*/

        private async void LunchBreakOnClick(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "This features is not available", "OK");
        }

        private async void Notification_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new notificationPage()); 
        }

        private async void SeeMore_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonthTracker());
        }


    }
}
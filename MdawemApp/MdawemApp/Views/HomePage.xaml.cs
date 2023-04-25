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
    public partial class HomePage : ContentPage
    {
        bool isCheckInTrue = false;
        bool isCheckInToggled = false;
        bool isCheckOutToggled = false;
        FirebaseHelper firebaseHelper;
        public HomePage()
        {
            InitializeComponent();
            firebaseHelper = new FirebaseHelper();
            //NavigationPage.SetHasBackButton(this, false);
            //NavigationPage.SetHasNavigationBar(this, true);
            SetInformations();
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasBackButton(this, false);
        }

        async void SetInformations()
        {
            var infos = await firebaseHelper.GetUserInformation();
            string name = $"{infos.FirstName}, {infos.LastName}";
            Name.Text = name;
            DateTime dateTime = DateTime.Now;
            TodayDate.Text = dateTime.ToString("dd MMMM yyyy");
        }

        private async void CheckIn_Clicked(object sender, EventArgs e)
        {
            if (isCheckInTrue)
            {
                await DisplayAlert("Alert", "You are already checked in, please check out first", "OK");
                return;
            }
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

                    await firebaseHelper.CheckIn(uid, location.Latitude, location.Longitude);
                    isCheckInTrue = true;
                    await DisplayAlert("Success", "Check-in Is Done", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Unable to get location", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
                isCheckInTrue = false;
            }
        }
        private async void CheckOut_Clicked(object sender, EventArgs e)
        {
            if (!isCheckInTrue)
            {
                await DisplayAlert("Alert", "You are already checked out, please check in first", "OK");
                return;
            }
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
                await firebaseHelper.CheckOut(uid);
                isCheckInTrue = false;
                await DisplayAlert("Success", "Check-out Is Done", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
                isCheckInTrue = true;
            }
		}

        private void CheckInOnClicked(object sender, EventArgs e)
        {
            if (!isCheckInToggled)
            {
                DateTime now = DateTime.Now;
                string formattedTime = now.ToString("h:mm");
                checkIn.Text = formattedTime;
                checkIn.IsVisible = true;
                CheckIn_Clicked(sender, e);
            }
            else
            {
                checkIn.Text = "";
                checkIn.IsVisible = false;
            }
            isCheckInToggled = !isCheckInToggled;
        }

        private void CheckOutOnClicked(object sender, EventArgs e)
        {
            if (!isCheckOutToggled)
            {
                DateTime now = DateTime.Now;
                string formattedTime = now.ToString("h:mm");
                checkOut.Text = formattedTime;
                checkOut.IsVisible = true;
                CheckOut_Clicked(sender, e);
            }
            else
            {
                checkOut.Text = "";
                checkOut.IsVisible = false;
            }
            isCheckOutToggled = !isCheckOutToggled;
        }

        private async void RequestOnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RequestForVacation());
        }

        private async void OnProfileIconClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        private async void LunchBreakOnClick(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "This Function is not available", "OK");
        }

        private async void Notification_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new notificationPage()); 
        }

    }
}
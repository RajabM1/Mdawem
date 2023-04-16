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
        FirebaseHelper firebaseHelper;
        public HomePage()
        {
            InitializeComponent();
            firebaseHelper = new FirebaseHelper();
            NavigationPage.SetHasNavigationBar(this, true);
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
		
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RequestForVacation());
        }
    }
}
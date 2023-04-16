using MdawemApp.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Globalization;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage
    {
        FirebaseHelper _userRepo;
        public Map()
        {
            InitializeComponent();
            _userRepo = new FirebaseHelper();
            //_userRepo.GetLocations("2023", "04");

             DisplayDeviceLocation();


        }
        public async Task<Location> GetDeviceLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.GetLocationAsync(request);
            return location;
        }
        private async void DisplayDeviceLocation()
        {
            DateTime now = DateTime.Now;
            CultureInfo culture = new CultureInfo("en-US");
            string formattedDate = now.ToString("yyyy/MM/dd", culture);
            var location = await _userRepo.GetLocations(formattedDate.Split('/')[0], formattedDate.Split('/')[1]);
            if(location.Count == 0)
            {
                await DisplayAlert("No check-in", "No check-in for today.", "cancle");
            } else
            {
                List<Pin> myPins = new List<Pin>();

                foreach (var loc in location)
                {
                    var pin = new Pin
                    {
                        Position = new Position(double.Parse(loc.Latitude), double.Parse(loc.Longitude)),
                        Label = "My Location",
                        Type = PinType.Place

                    };
                    myMap.Pins.Add(pin);
                    myPins.Add(pin);
                }
                double minLat = myPins.Min(pin => pin.Position.Latitude);
                double maxLat = myPins.Max(pin => pin.Position.Latitude);
                double minLon = myPins.Min(pin => pin.Position.Longitude);
                double maxLon = myPins.Max(pin => pin.Position.Longitude);

                double centerLat = (minLat + maxLat) / 2;
                double centerLon = (minLon + maxLon) / 2;

                var mapSpan = MapSpan.FromCenterAndRadius(new Position(centerLat, centerLon), Distance.FromMiles(1));
                myMap.MoveToRegion(mapSpan);
            }
            

        }
        
    }

}



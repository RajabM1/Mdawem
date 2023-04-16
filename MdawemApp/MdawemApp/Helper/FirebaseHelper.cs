using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using MdawemApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;



namespace MdawemApp.Helper
{

    public class FirebaseHelper
    {
        string webAPIKey = "AIzaSyDuxpf83oL4rNwmPBV06DEid9xUWPNyOWU";
        FirebaseAuthProvider authProvider;
        FirebaseClient client = new FirebaseClient(
                "https://mdawemh-default-rtdb.firebaseio.com/"
            );
        public FirebaseHelper()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        }


        public async Task<string> Login(string email, string password)
        {
            var token = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return token.FirebaseToken;
            }
            {
                return "";
            }
        }

        public void SignOut()
        {
            Application.Current.Properties.Remove("emailtxt");
            Application.Current.Properties.Remove("passwordtxt");
        }
        public async Task<bool> Register(string email, string password)
        {
            var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return true;
            }
            return false;
        }
       
        public async Task<List<LocationViewModel>> GetEmployeesLocations(string year, string month)
        {
            string attendancePath = $"attendance/{year}/{month}";
            var dataSnapshot = await client.Child("users").OnceAsync<object>();

            var locations = new List<LocationViewModel>();

            foreach (var childSnapshot in dataSnapshot)
            {
                var userId = childSnapshot.Key;
                var attendanceSnapshot = await client.Child($"users/{userId}/{attendancePath}").OnceAsync<object>();

                foreach (var attendanceChildSnapshot in attendanceSnapshot)
                {
                    var value = attendanceChildSnapshot.Object;
                    var locationJson = value.ToString();
                    var location = JsonConvert.DeserializeObject<LocationViewModel>(locationJson);
                    

                    var locationViewModel = new LocationViewModel
                    {
                        Date = location.Date,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                    };
                    DateTime now = DateTime.Now;
                    CultureInfo culture = new CultureInfo("en-US");
                    string formattedDate = now.ToString("yyyy/MM/dd", culture);
                    

                    if (locationViewModel.Date == formattedDate)
                     {
                          locations.Add(locationViewModel);
                      } 

                }
            }

            return locations;
        }
    }
}

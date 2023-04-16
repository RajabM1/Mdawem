using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace MdawemApp.Helper
{
    public class FirebaseHelper
    {
        string webAPIKey = "AIzaSyDuxpf83oL4rNwmPBV06DEid9xUWPNyOWU";
        FirebaseAuthProvider authProvider;
        FirebaseClient client = new FirebaseClient(
        "https://mdawemt-default-rtdb.firebaseio.com/"
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
                Application.Current.Properties["UID"] = token.User.LocalId;
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
            Application.Current.Properties.Remove("UID");

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

        public async Task<bool> CheckIn(string userId, double latitude, double longitude)
        {
            try
            {
                DateTime currentDate = DateTime.Now;

                string formattedDateWithOutDay = currentDate.ToString("yyyy/MM");
                string formattedDate = currentDate.ToString("yyyy/MM/dd");

                var AttendanceData = new
                {
                    Date = formattedDate,
                    Latitude = latitude,
                    Longitude = longitude,
                    TimeIn = currentDate.ToString("hh:mm:ss tt"),
                    TimeOut = ""
                };

                var result = await client
                .Child("users")
                .Child(userId)
                .Child("Attendance")
                .Child(formattedDateWithOutDay)
                .PostAsync(AttendanceData);
                Preferences.Set("attendanceKey", result.Key);

                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

                return false;
            }
        }
        public async Task<bool> CheckOut(string userId)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string formattedDate = currentDate.ToString("yyyy/MM/dd");
                string formattedDateWithOutDay = currentDate.ToString("yyyy/MM");
                string attendanceKey = Preferences.Get("attendanceKey", string.Empty);

                var attendanceSnapshot = await client
                     .Child("users")
                     .Child(userId)
                     .Child("Attendance")
                     .Child(formattedDateWithOutDay)
                     .Child(attendanceKey)
                     .OnceSingleAsync<Dictionary<string, object>>();

                if (attendanceSnapshot != null)
                {

                    var attendanceData = attendanceSnapshot;
                    attendanceData["TimeOut"] = currentDate.ToString("hh:mm:ss tt");

                    await client
                        .Child("users")
                        .Child(userId)
                        .Child("Attendance")
                        .Child(formattedDateWithOutDay)
                        .Child(attendanceKey)
                        .PutAsync(attendanceData);
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

                return false;
            }
        }
    }
}

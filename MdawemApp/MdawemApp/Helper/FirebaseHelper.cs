using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using MdawemApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<bool> ResetPassword(string email)
        {
            try
            {
                await authProvider.SendPasswordResetEmailAsync(email);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<Attendance>> GetAttendance(string userId, string year, string month)
        {

                string path = $"users/{userId}/locations/{year}/{month}";

                var dataSnapshot = await client.Child(path).OnceAsync<object>();

                if (!dataSnapshot.Any())
                {
                    return null;
                }

                var Attendances = new List<Attendance>();

                foreach (var childSnapshot in dataSnapshot)
                {
                    var value = childSnapshot.Object;
                    var valueJson = value.ToString();
                    var Attend = JsonConvert.DeserializeObject<Attendance>(valueJson);

                    var attendanceViewModel = new Attendance
                    {
                        Date = Attend.Date,
                        TimeIn = Attend.TimeIn,
                        TimeOut = Attend.TimeOut
                    };

                    Attendances.Add(attendanceViewModel);
                }
                return Attendances;
           
        }
        public async Task<bool> SaveRequestToFireBase(VactionRequestModel vactionRequestModel)
        {
            try
            {
                string UserID = "UVI1lkjyHsRcIFDlxamAGKTH9hF3";

                var data = await client.Child("users").
                    Child(UserID).
                    Child("Leaves").
                    Child(DateTime.Parse(vactionRequestModel.StartDate).Year.ToString()).
                    PostAsync(vactionRequestModel);

                if (!string.IsNullOrEmpty(data.Key))
                {
                    return true;
                }
            }
            catch (Exception excption)
            {
                Console.WriteLine($"Error: {excption.Message}");
            }

            return false;
        }

    }
}

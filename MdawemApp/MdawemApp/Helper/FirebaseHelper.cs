using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using MdawemApp.Models;
using MdawemApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
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
                Preferences.Set("token", token.FirebaseToken);
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
		
        public async Task<string> Register(string email, string password)
        {
            var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                var user = token.User;
                return user.LocalId;
            }
            return null;
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
                
               return true;
			}
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

                return false;
            }
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

                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

                return false;
            }


                return false;
        }


        public async Task<List<Attendance>> GetAttendance(string userId, string year, string month)
        {

                string path = $"users/{userId}/Attendance/{year}/{month}";

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
                var UserID = Application.Current.Properties["UID"].ToString();

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

        public async Task<List<VactionRequestModel>> GetLeaves(string userId, string year, string leaveType)
        {
            string path = $"users/{userId}/Leaves/{year}";
            var dataSnapshot = await client.Child(path).OnceAsync<object>();
            if (!dataSnapshot.Any())
            {
                return null;
            }

            var Leaves = new List<VactionRequestModel>();
            foreach (var childSnapshot in dataSnapshot)
            {
                var value = childSnapshot.Object;
                var valueJson = value.ToString();
                var Leave = JsonConvert.DeserializeObject<VactionRequestModel>(valueJson);

                if (string.IsNullOrEmpty(leaveType) || Leave.Type.Equals(leaveType, StringComparison.OrdinalIgnoreCase))
                {
                    var LeaveViewModel = new VactionRequestModel
                    {
                        Dateofrequest = Leave.Dateofrequest,
                        StartDate = Leave.StartDate,
                        EndDate = Leave.EndDate,
                        Type = Leave.Type,
                        Status = Leave.Status
                    };
                    Leaves.Add(LeaveViewModel);
                }
            }
            return Leaves;
        }

        public async Task<List<Attendance>> GetEmployeesLocations(string year, string month)
        {
            string attendancePath = $"Attendance/{year}/{month}";
            var dataSnapshot = await client.Child("users").OnceAsync<object>();

            var locations = new List<Attendance>();

            foreach (var childSnapshot in dataSnapshot)
            {
                var userId = childSnapshot.Key;
                var attendanceSnapshot = await client.Child($"users/{userId}/{attendancePath}").OnceAsync<object>();

                foreach (var attendanceChildSnapshot in attendanceSnapshot)
                {
                    var value = attendanceChildSnapshot.Object;
                    var locationJson = value.ToString();
                    var location = JsonConvert.DeserializeObject<Attendance>(locationJson);


                    var locationViewModel = new Attendance
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

        public async Task<List<object>> GetNotification()
        {
            DateTime currentDate = DateTime.Now;
            string CurrentYear = currentDate.Year.ToString();

            var dataSnapshot = await client.Child("users").OnceAsync<object>();

            string LeavesPath = $"Leaves/{CurrentYear}";

            var Vaction = new List<object>();

            foreach (var childSnapshot in dataSnapshot)
            {
                var userId = childSnapshot.Key;

                var LeavesSnapshot = await client.Child($"users/{userId}/{LeavesPath}").OnceAsync<object>();

                if (LeavesSnapshot.Count != 0)
                {
                    var DataSnapshot = await client.Child($"users/{userId}/PersonalInfo").OnceAsync<object>();

                    foreach (var LeavesChildSnapshot in LeavesSnapshot)
                    {
                        var value = LeavesChildSnapshot.Object;
                        var LeavesJson = value.ToString();
                        var Leaves = JsonConvert.DeserializeObject<VactionRequestModel>(LeavesJson);

                        var Notification = new NotificationData();

                        foreach (var Data in DataSnapshot)
                        {
                            var valueD = Data.Object;
                            var DataJson = valueD.ToString();
                            var Info = JsonConvert.DeserializeObject<Employee>(DataJson);

                            Notification.FirstName = Info.FirstName;
                            Notification.LastName = Info.LastName;
                        }
                        Notification.UserId = userId;
                        Notification.DateOfRequest = Leaves.Dateofrequest;
                        Notification.StartDate = Leaves.StartDate;
                        Notification.EndDate = Leaves.EndDate;
                        Notification.Type = Leaves.Type;
                        Notification.Status = Leaves.Status;
                        Notification.Reason = Leaves.Reason;
                        Notification.LeaveId = LeavesChildSnapshot.Key;

                        Vaction.Add(Notification);
                    }
                }
            }
            return Vaction;
        }
        public async Task<bool> UpdateVacationStatus(string Status, string userId,string LeaveID)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                //string formattedDate = currentDate.ToString("yyyy/MM/dd");
                //string formattedDateWithOutDay = currentDate.ToString("yyyy/MM");
                //string attendanceKey = Preferences.Get("LeavesKey", "Awaiting");

                var LeavesSnapshot = await client
                     .Child("users")
                     .Child(userId)
                     .Child("Leaves")
                     .Child("2023")
                     .Child(LeaveID)
                     .OnceSingleAsync<Dictionary<string, object>>();

                if (LeavesSnapshot != null)
                {
                    var attendanceData = LeavesSnapshot;
                    attendanceData["Status"] = Status;

                    await client
                        .Child("users")
                        .Child(userId)
                        .Child("Leaves")
                        .Child("2023")
                        .Child(LeaveID)
                        .PutAsync(attendanceData);
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



        public async Task<Employee> GetUserInformation()
        {
            string userId = Application.Current.Properties["UID"].ToString();

            try
            {
                var response = await client.Child("users").Child(userId).Child("PersonalInfo").OnceAsync<object>();
                foreach (var item in response)
                {
                    var value = item.Object;
                    var valueJson = value.ToString();
                    var personalInfo = JsonConvert.DeserializeObject<Employee>(valueJson);
                    return personalInfo;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user information for user {userId}: {ex.Message}");
                return null;
            }
        }
    }
}

        public async Task<bool> ChangePassword(string token, string newPassword)
        {
            try
            {
                await authProvider.ChangeUserPassword(token, newPassword);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }


        public async Task<List<Employee>> GetInfo()
        {
            var dataSnapshot = await client.Child("Employee").OnceAsync<object>();
            if (!dataSnapshot.Any())
            {
                return null;
            }

            var Info = new List<Employee>();
            foreach (var childSnapshot in dataSnapshot)
            {
                var value = childSnapshot.Object;
                var valueJson = value.ToString();
                var Infos = JsonConvert.DeserializeObject<Employee>(valueJson);



                var InfoViewModel = new Employee
                {

                    FirstName = Infos.FirstName,
                    Email = Infos.Email,
                    HomeAddress = Infos.HomeAddress,
                    PhoneNumber = Infos.PhoneNumber,
                    DateOfBirth = Infos.DateOfBirth,

                };
                Info.Add(InfoViewModel);

            }
            return Info;
        }
        public async Task UpdateEmployee(Employee updatedEmployee)
        {

            var employeeRef = client.Child("Employee");
            string employeeJson = JsonConvert.SerializeObject(updatedEmployee);
            await client.Child("Employee").PutAsync(employeeJson);
        }

    }
}



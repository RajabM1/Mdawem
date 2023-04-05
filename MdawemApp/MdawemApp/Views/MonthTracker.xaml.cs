using MdawemApp.Helper;
using MdawemApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonthTracker : ContentPage
    {
        FirebaseHelper _userRepo = new FirebaseHelper();
        List<Attendance> Attends = new List<Attendance>();
        public MonthTracker()
        {
            InitializeComponent();
            BindingContext = new MonthTrackerViewModel();
            GetAttendances();
        }

        

        public async void GetAttendances()
        {
            string userId = "h67eh6zGV6WqbBTEwPpMwDmTidx2";
            
            Attends = await _userRepo.GetAttendance(userId, "2023", "04", "03");

            foreach (Attendance location in Attends)
            {
                string Date = location.Date;
                string TimeIn = location.TimeIn;
                string TimeOut = location.TimeOut;

                



                grid.Children.Add(checkInLabel);
                grid.Children.Add(timeInLabel);
                grid.Children.Add(checkOutLabel);
                grid.Children.Add(timeOutLabel);

                frame.Content = grid;

                locationStackLayout.Children.Add(dateLabel);
                locationStackLayout.Children.Add(frame);

                AttendanceStackLayout.Children.Add(locationStackLayout);
            }
        }
    }
}
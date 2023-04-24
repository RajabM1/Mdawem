using Firebase.Auth;
using MdawemApp.Helper;
using MdawemApp.Models;
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
            DateTime currentDate = DateTime.Now;
            var CurrentMonthS = currentDate.ToString("MMMM");
            string CurrentYear = currentDate.Year.ToString();
            string monthString = currentDate.Month.ToString("D2");
            SelectedMonthYearLabel.Text = $"{CurrentMonthS} {CurrentYear}";

            DisplayAttendance(CurrentYear, monthString);
        }


        public async void DisplayAttendance(string year, string month)
        {

            var UID = Application.Current.Properties["UID"].ToString();

            Attends = await _userRepo.GetAttendance(UID, year, month);

            if (Attends != null)
            {
                Attends = Attends.OrderByDescending(a => a.Date).ToList();

                AttendanceStackLayout.Children.Clear();

                foreach (Attendance location in Attends)
                {
                    string Date = location.Date;
                    string TimeIn = location.TimeIn;
                    string TimeOut = location.TimeOut;


                    StackLayout locationStackLayout = new StackLayout();

                    Label dateLabel = new Label
                    {
                        Text = $"{Date}",
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        Margin = new Thickness(3, 5, 0, 5)
                    };

                    Frame frame = new Frame
                    {
                        HasShadow = true,
                        BorderColor = Color.FromHex("#CCC"),
                        Padding = new Thickness(10),
                        CornerRadius = 17,
                        Margin = new Thickness(0, 0, 0, 10)

                    };

                    Grid grid = new Grid
                    {
                        RowDefinitions =
                        {
                            new RowDefinition { Height = GridLength.Auto },
                            new RowDefinition { Height = GridLength.Auto },
                            new RowDefinition { Height = GridLength.Auto }
                        },
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = GridLength.Star },
                            new ColumnDefinition { Width = GridLength.Auto }
                        }
                    };

                    Label checkInLabel = new Label
                    {
                        Text = "Check-in"
                    };
                    Grid.SetRow(checkInLabel, 0);
                    Grid.SetColumn(checkInLabel, 0);

                    Label timeInLabel = new Label
                    {
                        Text = $"{TimeIn}"
                    };
                    Grid.SetRow(timeInLabel, 0);
                    Grid.SetColumn(timeInLabel, 1);

                    Label checkOutLabel = new Label
                    {
                        Text = "Checkout"
                    };
                    Grid.SetRow(checkOutLabel, 1);
                    Grid.SetColumn(checkOutLabel, 0);

                    Label timeOutLabel = new Label
                    {
                        Text = $"{TimeOut}"
                    };
                    Grid.SetRow(timeOutLabel, 1);
                    Grid.SetColumn(timeOutLabel, 1);



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
            else
            {
                AttendanceStackLayout.Children.Clear();

                await DisplayAlert("error", "Wrong Path", "OK");
            }
        }

        private async void Filter_Clicked(object sender, EventArgs e)
        {
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            int currentYear = DateTime.Now.Year;
            int minYear = 2023;
            int maxYear = 2030;

            string month = await DisplayActionSheet("Select Month", "Cancel", null, months);

            if (month != null && month != "Cancel")
            {
                int monthNumber = Array.IndexOf(months, month) + 1;
                string year = await DisplayActionSheet("Select Year", "Cancel", null, Enumerable.Range(minYear, maxYear - minYear + 1).Select(y => y.ToString()).ToArray());

                if (year != null && year != "Cancel")
                {
                    string monthString = monthNumber.ToString("D2");
                    SelectedMonthYearLabel.Text = $"{month} {year}";

                    DisplayAttendance(year, monthString);

                }
            }
        }
    }
}
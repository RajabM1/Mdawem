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


                StackLayout locationStackLayout = new StackLayout();

                Label dateLabel = new Label
                {
                    Text = $"{Date}",
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                Frame frame = new Frame
                {
                    HasShadow = true,
                    BorderColor = Color.FromHex("#CCC"),
                    Padding = new Thickness(10),
                    CornerRadius = 17
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
    }
}
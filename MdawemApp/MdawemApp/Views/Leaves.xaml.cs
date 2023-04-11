using Firebase.Database;
using MdawemApp.Helper;
using MdawemApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Leaves : ContentPage
    {
        FirebaseHelper _userRepo = new FirebaseHelper();
        List<LeaveManagement> leave = new List<LeaveManagement>();
        public Leaves()
        {
            InitializeComponent();
            DateTime currentDate = DateTime.Now;
            string CurrentYear = currentDate.Year.ToString();
            string monthString = currentDate.Month.ToString("D2");

            DisplayAttendance("7XQCOtDQ6FXS4YtONJF1XgTUKSZ2", CurrentYear, monthString,null);
        }
        public async void DisplayAttendance(string UserId, string year, string month,string type)
        {
            string userId = "7XQCOtDQ6FXS4YtONJF1XgTUKSZ2";

           var leave = await _userRepo.GetLeaves(userId, year, month, type);

           
                LeavesListView.Children.Clear();

                foreach (LeaveManagement leaves in leave)
                {
                    string Date = leaves.Date;
                    string FromTime = leaves.FromTime;
                    string ToTime = leaves.ToTime;
                    string TypeOfLeaves = leaves.TypeOfLeaves;
                    string Status = leaves.Status;


                StackLayout LeaveStackLayout = new StackLayout();

                Grid grid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                    },
                };

                Label fromLabel = new Label
                    {
                        Text = "From:",
                        Margin = new Thickness(0),
                        Padding=new Thickness(5,0,0,0),
                        FontAttributes = FontAttributes.Bold
                        
                };
                    Grid.SetRow(fromLabel, 0);
                    Grid.SetColumn(fromLabel, 0);

                    Label timefromLabel = new Label
                    {
                        Text = $"{FromTime}",
                        Margin = new Thickness(0),
                    };
                    Grid.SetRow(timefromLabel, 0);
                    Grid.SetColumn(timefromLabel, 1);

                    Label toLabel = new Label
                    {
                        Text = "To:",
                        Margin = new Thickness(0),
                        Padding = new Thickness(5, 0, 0, 0),
                        FontAttributes = FontAttributes.Bold
                    };
                    Grid.SetRow(toLabel, 1);
                    Grid.SetColumn(toLabel, 0);

                    Label timetoLabel = new Label
                    {
                        Text = $"{ToTime}",
                        Margin = new Thickness(0),
                    };
                    Grid.SetRow(timetoLabel, 1);
                    Grid.SetColumn(timetoLabel, 1);

                    Label typeLabel = new Label
                    {
                        Text = "Type:",
                        Margin = new Thickness(0),
                        Padding = new Thickness(5, 0, 0, 0),
                        FontAttributes = FontAttributes.Bold
                    };
                    Grid.SetRow(typeLabel, 2);
                    Grid.SetColumn(typeLabel, 0);

                    Label typeOfLabel = new Label
                    {
                        Text = $"{TypeOfLeaves}",
                        Margin = new Thickness(0),
                    };
                    Grid.SetRow(typeOfLabel, 2);
                    Grid.SetColumn(typeOfLabel, 1);

                    grid.Children.Add(fromLabel);
                    grid.Children.Add(toLabel);
                    grid.Children.Add(typeLabel);
                    grid.Children.Add(timefromLabel);
                    grid.Children.Add(timetoLabel);
                    grid.Children.Add(typeOfLabel);

                    Frame StatusFrame = new Frame
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Padding = new Thickness(10),
                        Margin = new Thickness(60, -10, 0, 10),
                        CornerRadius = 10,
                        Content = new Label { Text = $"{Status}" ,TextColor=Color.White}
                    };
                    Grid.SetRow(StatusFrame, 1);
                    Grid.SetColumn(StatusFrame, 2);
                    Grid.SetRowSpan(StatusFrame, 2);

                    grid.Children.Add(StatusFrame);

                    Frame mainFrame = new Frame
                    {
                        CornerRadius = 10,
                        Content = grid,
                        Margin= new Thickness(20,10,20,10),
                        BackgroundColor= Color.FromHex("#E8E8E8")
                    };

                    Label dateLabel = new Label
                    {
                        Text = $"{Date}",
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(20, 0, 20, 0),

                    };
                    if(leaves.Status== "Awaiting")
                    {
                        StatusFrame.BackgroundColor = Color.FromHex("#ffc107");
                    }
                    else if (leaves.Status == "Approved")
                    {
                        StatusFrame.BackgroundColor = Color.FromHex("#198754"); 
                    }
                    else if (leaves.Status == "Declined")
                    {
                        StatusFrame.BackgroundColor = Color.FromHex("#dc3545");
                    }

                    LeaveStackLayout.Children.Add(dateLabel);
                    LeaveStackLayout.Children.Add(mainFrame);

                    LeavesListView.Children.Add(LeaveStackLayout);

                }
            
            
        }
        private async void Plus_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RequestForVacation());
        }
        private void LeaveType_Clicked(string leaveType)
        {
            DateTime currentDate = DateTime.Now;
            string currentYear = currentDate.Year.ToString();
            string monthString = currentDate.Month.ToString("D2");

            DisplayAttendance("7XQCOtDQ6FXS4YtONJF1XgTUKSZ2", currentYear, monthString, leaveType);
        }
        private void ALL_Clicked(object sender, EventArgs e)
        {
            LeaveType_Clicked(null);
        }

        private void Casual_Clicked(object sender, EventArgs e)
        {
            LeaveType_Clicked("Casual");
        }

        private void Sick_Clicked(object sender, EventArgs e)
        {
            LeaveType_Clicked("Sick");
        }
    }
}
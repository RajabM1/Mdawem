﻿using Firebase.Auth;
using MdawemApp.Helper;
using MdawemApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class notificationPage : ContentPage
    {
        FirebaseHelper _userRepo = new FirebaseHelper();

        public notificationPage()
        {
            InitializeComponent();
            
            DisplayNotification();
            
        }
        public async void DisplayNotification()
        {
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;
            var Notification = await _userRepo.GetNotification();


            if (Notification != null)
            {
                NotificationsListView.Children.Clear();

                foreach (NotificationData Notifications in Notification)
                {
                    if (Notifications.Status == "Awaiting") 
                    {
                        string Content = Notifications.FirstName + " " + Notifications.LastName;
                        string Time = Notifications.DateOfRequest;

                        StackLayout NotificationStackLayout = new StackLayout();


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
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = GridLength.Auto }
                    },
                        };
                        Label ContentLabel = new Label
                        {
                            Text = $"{Content}",
                            Margin = new Thickness(0),
                            FontAttributes = FontAttributes.Bold
                        };
                        Grid.SetRow(ContentLabel, 0);
                        Grid.SetColumn(ContentLabel, 1);

                        Label StatusLabel = new Label
                        {
                            Text = "has requested for vacation.",
                            Margin = new Thickness(0),
                            TextColor = Color.Gray,
                        };
                        Grid.SetRow(StatusLabel, 1);
                        Grid.SetColumn(StatusLabel, 1);

                        Label timeLabel = new Label
                        {
                            Text = $"{Time}",
                            Margin = new Thickness(0),
                        };
                        Grid.SetRow(timeLabel, 2);
                        Grid.SetColumn(timeLabel, 1);

                        Image moreIcon = new Image
                        {
                            Source = "moreIcon.png",
                            HeightRequest = 30,
                            WidthRequest = 30,
                        };

                        Grid.SetRow(moreIcon, 0);
                        Grid.SetColumn(moreIcon, 2);
                        Grid.SetRowSpan(moreIcon, 3);

                        var moreIconTapGestureRecognizer = new TapGestureRecognizer();
                        moreIconTapGestureRecognizer.Tapped += async (s, e) => 
                        {
                            string action = await DisplayActionSheet("Title", null, null, "Approved", "Declined", "Dismissed");
                            if (action == "Approved")
                            {
                                await _userRepo.UpdateVacationStatus("Approved", Notifications.UserId, Notifications.LeaveId);
                            }
                            else if (action == "Declined")
                            {
                                await _userRepo.UpdateVacationStatus("Declined", Notifications.UserId, Notifications.LeaveId);
                            }
                            else
                            {
                                return;
                            }
                            await Navigation.PushAsync(new notificationPage());
                            Navigation.RemovePage(this);

                        };
                        moreIcon.GestureRecognizers.Add(moreIconTapGestureRecognizer);

                        Image image = new Image
                        {
                            Source = "profile.png"
                        };
                        Grid.SetRow(image, 0);
                        Grid.SetColumn(image, 0);
                        Grid.SetRowSpan(image, 3);

                        grid.Children.Add(StatusLabel);
                        grid.Children.Add(ContentLabel);
                        grid.Children.Add(timeLabel);
                        grid.Children.Add(moreIcon);
                        grid.Children.Add(image);
                                             
                        Frame mainFrame = new Frame
                        {
                            CornerRadius = 10,
                            Content = grid,
                            Margin = new Thickness(20, 10, 20, 10),
                            BackgroundColor = Color.FromHex("#E8E8E8")
                        };
                   
                        NotificationStackLayout.Children.Add(mainFrame);

                        var notificationTapGestureRecognizer = new TapGestureRecognizer();
                        notificationTapGestureRecognizer.Tapped += async (s, e) =>
                        {
                             await DisplayAlert("Vacation Details",
                                $"First Name                    {Notifications.FirstName}\n" +
                                $"Last Name                    {Notifications.LastName}\n" +
                                $"Date of Request           {Notifications.DateOfRequest}\n" +
                                $"Type of Request           {Notifications.Type}\n" +
                                $"Status                            {Notifications.Status}\n" +
                                $"Reason                          {Notifications.Reason}\n" +
                                $"From                              {Notifications.StartDate}\n" +
                                $"To                                   {Notifications.EndDate}\n"
                                ,"cancel");
                        };
                        NotificationStackLayout.GestureRecognizers.Add(notificationTapGestureRecognizer);

                        NotificationsListView.Children.Add(NotificationStackLayout);
                    }
                    
                }
            }
            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }
    }
}
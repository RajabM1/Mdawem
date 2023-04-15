using MdawemApp.Helper;
using MdawemApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestForVacation : ContentPage
    {
        private bool _isCheckedStartDate = false;

        private bool _isCheckedEndDate = false;
        FirebaseHelper firebaseHelper;
        private const int MaxDaysAllowed = 14;

        public RequestForVacation()
        {

            InitializeComponent();
            firebaseHelper = new FirebaseHelper();
        }



        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!_isCheckedStartDate)
            {
                if (DatePickerEnd.IsVisible)
                {
                    DatePickerEnd.IsVisible = false;
                }
                DatePicker.IsVisible = true;
            }
            else
            {
                DatePicker.IsVisible = false;
                _isCheckedStartDate = false;
            }
        }

        private void MyCalendar_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MyCalendar.SelectedDate))
            {
                if (MyCalendar.SelectedDate != null && MyCalendar.SelectedDate.ToString() != "")
                {
                    startDate.Text = MyCalendar.SelectedDate.ToString("ddd, dd MMM yyyy");
                }
            }
        }


        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            if (!_isCheckedEndDate)
            {
                if (DatePicker.IsVisible)
                {
                    DatePicker.IsVisible = false;
                }
                DatePickerEnd.IsVisible = true;
            }
            else
            {
                DatePickerEnd.IsVisible = false;
                _isCheckedEndDate = false;
            }
        }

        private async void MyCalendarEnd_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MyCalendarEnd.SelectedDate))
            {
                if (MyCalendarEnd.SelectedDate != null && MyCalendarEnd.SelectedDate.ToString() != "")
                {

                    endDate.Text = MyCalendarEnd.SelectedDate.ToString("ddd, dd MMM yyyy");
                    string leaveType = GetSelectedLeaveType();
                    string startDateTime = startDate.Text;
                    string endDateTime = endDate.Text;
                    string reason = causeEntry.Text;
                    string errorMessage = ValidateInput(leaveType, startDateTime, endDateTime, reason);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        await DisplayAlert("Validation Error", errorMessage, "OK");
                        return;
                    }
                }
            }
        }


        private async void SubmitRequest(object sender, EventArgs e)
        {
            try
            {
                string leaveType = GetSelectedLeaveType();
                string startDateTime = startDate.Text;
                string endDateTime = endDate.Text;
                string reason = causeEntry.Text;
                string errorMessage = ValidateInput(leaveType, startDateTime, endDateTime, reason);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    await DisplayAlert("Validation Error", errorMessage, "OK");
                    return;
                }

                var request = new VactionRequestModel
                {
                    Type = leaveType,
                    StartDate = startDateTime,
                    EndDate = endDateTime,
                    Reason = reason,


                };

                bool isSaved = await firebaseHelper.SaveRequestToFireBase(request);
                string message = isSaved ? "Your request has been submitted." : "Failed to save your request.";
                await DisplayAlert(isSaved ? "Success" : "Error", message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private string GetSelectedLeaveType()
        {
            if (casualRadioButton.IsChecked == true)
            {
                return casualRadioButton.Content.ToString();
            }
            else if (sickRadioButton.IsChecked == true)
            {
                return sickRadioButton.Content.ToString();
            }
            return string.Empty;
        }

        private string ValidateInput(string leaveType, string startDate, string endDate, string reason)
        {
            if (string.IsNullOrEmpty(leaveType))
            {
                return "Please select a leave type.";
            }
            if (DateTime.Parse(startDate) < DateTime.Today)
            {
                return "Start date cannot be in the past.";
            }
            if (DateTime.Parse(endDate) <= DateTime.Parse(startDate))
            {
                return "End date cannot be before start date.";
            }
            int daysRequested = (DateTime.Parse(endDate) - DateTime.Parse(startDate)).Days;
            if (daysRequested > MaxDaysAllowed)
            {
                return $"You are only allowed to request up to {MaxDaysAllowed} days of vacation";
            }
            string buttonTitle = $"Apply to  ({daysRequested} days Leave)";
            btntext.Text = buttonTitle;
            if (string.IsNullOrEmpty(reason))
            {
                return "Please enter a reason for the vacation.";
            }
            return string.Empty;
        }
    }
}

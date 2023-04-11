using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public RequestForVacation()
        {
            InitializeComponent();
            
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

        private void MyCalendarEnd_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MyCalendarEnd.SelectedDate))
            {
                if (MyCalendarEnd.SelectedDate != null && MyCalendarEnd.SelectedDate.ToString() != "")
                {
                    endDate.Text = MyCalendarEnd.SelectedDate.ToString("ddd, dd MMM yyyy");
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
    }
}
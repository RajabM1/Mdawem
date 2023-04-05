using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MdawemApp.ViewModels
{
    public class MonthTrackerViewModel
    {
        public string CurrentMonth { get; set; }
        public int CurrentYear { get; set; }

        public MonthTrackerViewModel()
        {
            // Set culture to English (United States)
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            // Retrieve current date and time
            DateTime currentDate = DateTime.Now;

            // Get name of current month and current year
            CurrentMonth = currentDate.ToString("MMMM");
            CurrentYear = currentDate.Year;
        }
    }
}

using Firebase.Auth;
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
	public partial class NotificationDetails : ContentPage
	{
        FirebaseHelper firebaseHelper=new FirebaseHelper();
		public NotificationDetails (NotificationData message)
		{
			InitializeComponent ();
			BindingContext = message;
		}

        private async void Accept_Clicked(object sender, EventArgs e)
        {
      
            await firebaseHelper.UpdateVacationStatus("Approved", UserID.Text, LeaveId.Text);
            await Navigation.PopAsync ();    
        }

        private async void Reject_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.UpdateVacationStatus("Declined", UserID.Text, LeaveId.Text);
            await Navigation.PopAsync();

        }
    }
}
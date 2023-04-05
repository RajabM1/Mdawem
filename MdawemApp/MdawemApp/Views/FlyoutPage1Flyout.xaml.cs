using MdawemApp.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutPage1Flyout : ContentPage
    {
        FirebaseHelper firebaseHelper;

        public ListView ListView;

        public FlyoutPage1Flyout()
        {
            InitializeComponent();
            firebaseHelper = new FirebaseHelper();
        }
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is FlyoutItemPage item)
            {
                await Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetPage));
                listview.SelectedItem = null;

            }
        }

        private async void LogOut_Clicked(object sender, EventArgs e)
        {
           firebaseHelper.SignOut();
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }
    }

}
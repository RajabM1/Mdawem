using MdawemApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MdawemApp
{
    public partial class App : Application
    {
      
        public App()
        {
            InitializeComponent();
             //var tabbedPage = new TabbedPage1();
             //tabbedPage.CurrentPage = tabbedPage.Children[2]; // Set the second page as the current page
             //MainPage = tabbedPage;
            MainPage = new NavigationPage(new LogInPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

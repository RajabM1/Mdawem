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
    public partial class TabbedPage1 : TabbedPage
    {
        public TabbedPage1 ()
        {
            InitializeComponent();
            Children.Add(new NavigationPage(new HomePage()) { Title = "Home", IconImageSource = "home.png" });
            Children.Add(new NavigationPage(new ProfilePage()) { Title = "Profile", IconImageSource = "profile.png" });
            Children.Add(new NavigationPage(new notificationPage()) { Title = "Notification", IconImageSource = "notification.png" });
            Children.Add(new NavigationPage(new FlyoutPage1Flyout()) { Title = "More", IconImageSource = "more.png" });
        }
    }
}
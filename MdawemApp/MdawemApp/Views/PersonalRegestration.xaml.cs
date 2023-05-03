using MdawemApp.ViewModels;
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
    public partial class PersonalRegestration : ContentPage
    {
        public PersonalRegestration()
        {
            InitializeComponent();
            BindingContext = new PersonalRegistrationViewModels();
        }

        private void NextPage(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new WorkRegestration());
        }
    }
}
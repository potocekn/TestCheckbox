using System;
using Xamarin.Forms;
using System.Globalization;
using Xamarin.Forms.Xaml;

namespace TestCheckbox
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.DeviceCultureInfo;
            MainPage = new NavigationPage(new MainPage(this));
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

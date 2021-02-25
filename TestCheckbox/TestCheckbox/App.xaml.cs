using System;
using Xamarin.Forms;
using System.Globalization;
using Xamarin.Forms.Xaml;
using System.Threading;
using AppBase.Resources;

namespace TestCheckbox
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.DeviceCultureInfo;
            CultureInfo language = new CultureInfo("en");
            Thread.CurrentThread.CurrentUICulture = language;
            AppResources.Culture = language;
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

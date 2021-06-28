using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBase.ViewModels;
using AppBaseNamespace;
using AppBaseNamespace.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBase
{
    /// <summary>
    /// Class used for displaying the settings menu. The menu consists of application language settings, 
    /// Resource format and languages settings and update interval settings.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage(App app, MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();            
            BindingContext = new SettingsPageViewModel(app,Navigation, mainPageViewModel);
        }
    }
}
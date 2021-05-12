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
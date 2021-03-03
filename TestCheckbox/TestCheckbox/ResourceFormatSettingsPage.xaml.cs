using AppBaseNamespace.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBaseNamespace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResourceFormatSettingsPage : ContentPage
    {        
        public ResourceFormatSettingsPage(App app, MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            string[] items = new string[] { "English", "Czech", "German" };
            BindingContext = new ResourceFormatSettingsPageViewModel(items, this);            
        }
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            this.DisplayAlert("", "Checked changed", "OK");
        }
    }
}
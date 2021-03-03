using AppBase.ViewModels;
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
            
            List<string> languages = new List<string> { "English", "Czech", "German" };
            List<ResourceFormatSettingsItemViewModel> switches = new List<ResourceFormatSettingsItemViewModel>();
            
            switches.Add(new ResourceFormatSettingsItemViewModel(wifiSwitch, "wifi"));
            switches.Add(new ResourceFormatSettingsItemViewModel(pdfSwitch, pdfLabel.Text));
            switches.Add(new ResourceFormatSettingsItemViewModel(htmlSwitch, htmlLabel.Text));
            switches.Add(new ResourceFormatSettingsItemViewModel(odtSwitch, odtLabel.Text));

            BindingContext = new ResourceFormatSettingsPageViewModel(app, mainPageViewModel, languages, switches);

            if (app.userSettings.DownloadOnlyWithWifi) wifiSwitch.IsToggled = true;
            foreach (var item in app.userSettings.Formats)
            {
                if (item == pdfLabel.Text) pdfSwitch.IsToggled = true;
                if (item == odtLabel.Text) odtSwitch.IsToggled = true;
                if (item == htmlLabel.Text) htmlSwitch.IsToggled = true;
            }
                       
        }
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            (BindingContext as ResourceFormatSettingsPageViewModel).OnCheckBoxCheckedChanged(sender, e);
        }

        void OnToggled(object sender, ToggledEventArgs e)
        {
            (BindingContext as ResourceFormatSettingsPageViewModel).OnToggled(sender, e);
        }
    }
}
using AppBase.ViewModels;
using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResourcesFormatFirstRunPage : ContentPage
    {
        public ResourcesFormatFirstRunPage(App app)
        {
            InitializeComponent();
            List<ResourceFormatSettingsItem> switches = new List<ResourceFormatSettingsItem>();
            switches.Add(new ResourceFormatSettingsItem(wifiSwitch, "wifi"));
            switches.Add(new ResourceFormatSettingsItem(pdfSwitch, pdfLabel.Text));
            switches.Add(new ResourceFormatSettingsItem(htmlSwitch, htmlLabel.Text));
            switches.Add(new ResourceFormatSettingsItem(odtSwitch, odtLabel.Text));
            BindingContext = new ResourcesFormatFirstRunPageViewModel(app, Navigation, switches);
        }

        void OnToggled(object sender, ToggledEventArgs e)
        {
            (BindingContext as ResourcesFormatFirstRunPageViewModel).OnToggled(sender, e);
        }       
    }
}
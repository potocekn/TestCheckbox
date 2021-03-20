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
            List<ResourceFormatSettingsItemViewModel> switches = new List<ResourceFormatSettingsItemViewModel>();
            switches.Add(new ResourceFormatSettingsItemViewModel(pdfSwitch, pdfLabel.Text));
            switches.Add(new ResourceFormatSettingsItemViewModel(htmlSwitch, htmlLabel.Text));
            switches.Add(new ResourceFormatSettingsItemViewModel(odtSwitch, odtLabel.Text));
            BindingContext = new ResourcesFormatFirstRunPageViewModel(app, Navigation, switches);
        }

        void OnToggled(object sender, ToggledEventArgs e)
        {
            (BindingContext as ResourcesFormatFirstRunPageViewModel).OnToggled(sender, e);
        }
    }
}
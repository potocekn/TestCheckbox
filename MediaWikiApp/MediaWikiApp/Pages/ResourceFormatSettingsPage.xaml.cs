using AppBase.Helpers;
using AppBase.Models;
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
    /// <summary>
    /// Class representing the page that contains format and language settings of the resources.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResourceFormatSettingsPage : ContentPage
    {       
        public ResourceFormatSettingsPage(App app, MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();            
            
            List<ResourceFormatSettingsItem> switches = new List<ResourceFormatSettingsItem>();
            
            switches.Add(new ResourceFormatSettingsItem(wifiSwitch, Constants.WIFI_TOGGLE_NAME));
            switches.Add(new ResourceFormatSettingsItem(pdfSwitch, pdfLabel.Text));
            switches.Add(new ResourceFormatSettingsItem(htmlSwitch, htmlLabel.Text));
            switches.Add(new ResourceFormatSettingsItem(odtSwitch, odtLabel.Text));

            BindingContext = new ResourceFormatSettingsPageViewModel(app, this, app.availableLanguages, switches);

            if (app.userSettings.DownloadOnlyWithWifi) wifiSwitch.IsToggled = true;
            foreach (var item in app.userSettings.Formats)
            {
                if (item == pdfLabel.Text) pdfSwitch.IsToggled = true;
                if (item == odtLabel.Text) odtSwitch.IsToggled = true;
                if (item == htmlLabel.Text) htmlSwitch.IsToggled = true;
            }
                       
        }

        /// <summary>
        /// Method used when a checkbox changes its checked status.
        /// </summary>
        /// <param name="sender">Checkbox that changes status</param>
        /// <param name="e">Event arguments</param>
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            (BindingContext as ResourceFormatSettingsPageViewModel).OnCheckBoxCheckedChanged(sender, e);
        }

        /// <summary>
        /// Method used when a switch changes its toggled status.
        /// </summary>
        /// <param name="sender">Switch that changed status</param>
        /// <param name="e">Event arguments</param>
        void OnToggled(object sender, ToggledEventArgs e)
        {
            (BindingContext as ResourceFormatSettingsPageViewModel).OnToggled(sender, e);
        }

        /// <summary>
        /// Method used for checking checkbox when label next to it was clicked on.
        /// </summary>
        /// <param name="sender">Label that was clicked on</param>
        /// <param name="e">Event arguments</param>
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            (BindingContext as ResourceFormatSettingsPageViewModel).TapGestureRecognizer_Tapped(sender, e);
        }

        /// <summary>
        /// Method used for requesting an update of the resources.
        /// </summary>
        /// <param name="sender">Update request button</param>
        /// <param name="e">Event arguments</param>
        private void RequestUpdateButton_Clicked(object sender, EventArgs e)
        {
            (BindingContext as ResourceFormatSettingsPageViewModel).RequestUpdate();
        }
    }
}
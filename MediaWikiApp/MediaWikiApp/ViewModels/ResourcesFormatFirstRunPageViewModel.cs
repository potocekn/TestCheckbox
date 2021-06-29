using AppBase.Helpers;
using AppBase.Models;
using AppBase.Pages;
using AppBase.Resources;
using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// Class that represents view model for the format of resources page 
    /// that is displayed on the very first run of the application.
    /// This view model sets the language of the application based on user input.
    /// </summary>
    public class ResourcesFormatFirstRunPageViewModel
    {
        public List<ResourceFormatSettingsItem> Switches { get; }
        App app;
        public Command GoToNextPage { get; set; }

        public ResourcesFormatFirstRunPageViewModel(App app, INavigation navigation, List<ResourceFormatSettingsItem> switches)
        {
            this.app = app;
            Switches = switches;
            
            GoToNextPage = new Command(() => {
                navigation.PushAsync(new UpdateIntervalFirstRunPage(app));
            });
        }

        

        /// <summary>
        /// Method that handles toggled changes of the switch.
        /// </summary>
        /// <param name="sender">switch that changed status</param>
        /// <param name="e">event args</param>
        public void OnToggled(object sender, ToggledEventArgs e)
        {
            foreach (var item in Switches)
            {
                if (item.CorrespondingSwitch == (sender as Switch))
                {
                    if (item.Name == Constants.WIFI_TOGGLE_NAME)
                    {
                        HandleWifiChange((sender as Switch).IsToggled);
                    }
                    else
                    {
                        HandleFormatChange(item.Name, (sender as Switch).IsToggled);
                    }
                }
            }
        }


        /// <summary>
        /// Method that handles change of toggled property of the wifi switch.
        /// </summary>
        /// <param name="isToggled">bool representing is switch is toggled or not</param>
        void HandleWifiChange(bool isToggled)
        {
            app.userSettings.DownloadOnlyWithWifi = isToggled;
            app.SaveUserSettings();
        }

        /// <summary>
        /// Method that handles format change. If user setting do not contain this format, the format is added. 
        /// If user setting contain the format, the format is removed.
        /// Updated user settings are saved.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isToggled"></param>
        void HandleFormatChange(string name, bool isToggled)
        {
            if (isToggled)
            {
                if (!app.userSettings.Formats.Contains(name))
                {
                    app.userSettings.Formats.Add(name);
                }
            }
            else
            {
                if (app.userSettings.Formats.Contains(name))
                {
                    app.userSettings.Formats.Remove(name);
                }
            }
            app.SaveUserSettings();
        }
    }
}

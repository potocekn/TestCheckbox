using System.Collections.Generic;
using AppBaseNamespace.ViewModels;
using System.Linq;
using Xamarin.Forms;
using AppBaseNamespace.Models;
using AppBase.Resources;
using AppBase.Helpers;
using System.IO;
using System;
using System.Globalization;
using AppBase.ViewModels;
using AppBase.Models;

namespace AppBaseNamespace
{
    /// <summary>
    /// Class representing view model for resource format settings page.
    /// The model remembers all switches and all languages of the resources.
    /// </summary>
    internal class ResourceFormatSettingsPageViewModel
    {
        public List<ResourceFormatSettingsItem> Switches { get; }
        public List<LanguageSettingsItem> Languages { get; set; }
        ResourceFormatSettingsPage Page { get; set; }
        App app;        
        public ResourceFormatSettingsPageViewModel(App app, ResourceFormatSettingsPage page, List<string> languages, List<ResourceFormatSettingsItem> switches)
        {
            this.app = app;
            this.Page = page;
            
            Languages = languages
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new LanguageSettingsItem()
                {
                    IsChecked = app.userSettings.ChosenResourceLanguages.Contains(x),
                    EnglishName = x,
                    Value = LanguagesTranslationHelper.ReturnTranslation(x),
                    WasUpdated = false, 
                    Shortcut = LanguagesTranslationHelper.GetLanguageShortcut(x)
                })
                .ToList();
            Switches = switches;            
        }

       

        /// <summary>
        /// Method that handles toggle change of the switch.
        /// </summary>
        /// <param name="sender">switch that changes status</param>
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
        /// Method that handles change of the toggled property for the format switch.
        /// </summary>
        /// <param name="name">name of the format</param>
        /// <param name="isToggled">bool representing is switch is toggled or not</param>
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

        /// <summary>
        /// Method used for requesting an update of the resources.
        /// </summary>
        internal async void RequestUpdate()
        {
            await RequestUpdateHelpers.RequestUpdate(Page, app, Languages);
        }

        /// <summary>
        /// Method that handles checkbox change for languages of resources.
        /// </summary>
        /// <param name="sender">checkbox that changed</param>
        /// <param name="e">event args</param>
        public void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            foreach (var item in Languages)
            {
                if (item.EnglishName == ((sender as CheckBox).BindingContext as LanguageSettingsItem).EnglishName)
                {
                    if ((sender as CheckBox).IsChecked && !app.userSettings.ChosenResourceLanguages.Contains(item.EnglishName))
                    {
                        app.userSettings.ChosenResourceLanguages.Add(item.EnglishName);
                        app.SaveUserSettings();
                        break;
                    }
                    else if (!(sender as CheckBox).IsChecked && app.userSettings.ChosenResourceLanguages.Contains(item.EnglishName))
                    {
                        app.userSettings.ChosenResourceLanguages.Remove(item.EnglishName);
                        app.SaveUserSettings();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Method that ensures if label was clicked on, checkbox next to it will be updated
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Label label = (sender as Label);
            foreach (var item in Languages)
            {
                if (label.Text == item.EnglishName)
                {
                    item.IsChecked = !item.IsChecked;
                    break;
                }
            }
        }
    }
}
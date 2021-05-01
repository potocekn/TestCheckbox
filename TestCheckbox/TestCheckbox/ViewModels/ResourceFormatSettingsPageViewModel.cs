using System.Collections.Generic;
using AppBaseNamespace.ViewModels;
using System.Linq;
using Xamarin.Forms;
using AppBase.ViewModels;
using AppBase.Resources;
using AppBase.Helpers;
using System.IO;
using AppBase.UserSettingsHelpers;

namespace AppBaseNamespace
{
    /// <summary>
    /// Class representing view model for resource format settings page. The model remembers all switches and all languages of the resources.
    /// </summary>
    internal class ResourceFormatSettingsPageViewModel
    {
        public List<ResourceFormatSettingsItem> Switches { get; }
        public List<CheckBoxItem> Languages { get; set; }
        App app;
        MainPageViewModel mainPageViewModel;
        public ResourceFormatSettingsPageViewModel(App app, MainPageViewModel mainPageViewModel, List<string> languages, List<ResourceFormatSettingsItem> switches)
        {
            this.app = app;
            this.mainPageViewModel = mainPageViewModel;
            
            Languages = languages
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new CheckBoxItem()
                {
                    IsChecked = app.userSettings.ChosenResourceLanguages.Contains(x),
                    Value = x,
                    LabelText = LanguagesTranslationHelper.ReturnTranslation(x),
                    CheckedChangedCommand = new Command(() => {                        
                    })
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
                    if (item.Name == "wifi")
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
        /// Methoid that handles change of the toggled property for the format switch.
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
                    //download resources
                    switch (name)
                    {
                        case "HTML":
                            UpdateSyncHelpers.SaveHtmlToDbs(); /////////////////for now
                            break;
                        case "PDF":
                            UpdateSyncHelpers.DownloadTestFiles(app);/////////////////for now
                            break;
                        case "ODT":
                            UpdateSyncHelpers.DownloadTestFiles(app);/////////////////for now
                            break;
                        default:
                            break;
                    }                     
                }
            }
            else
            {
                if (app.userSettings.Formats.Contains(name))
                {
                    app.userSettings.Formats.Remove(name);
                    //remove resources
                    switch (name)
                    {
                        case "HTML":
                            RemoveHTMLs();
                            break;
                        case "PDF":
                            RemoveFiles(app.resourcesPDF);
                            break;
                        case "ODT":
                            RemoveFiles(app.resourcesODT); 
                            break;
                        default:
                            break;
                    }

                }
            }
            app.SaveUserSettings();
        }

        void RemoveHTMLs()
        {
            var records = App.Database.GetPagesAsync();
            foreach (var item in records.Result)
            {
                App.Database.DeletePageAsync(item);
            }
        }

        void RemoveFiles(List<ResourcesInfoPDF> list)
        {
            foreach (var item in list)
            {
                File.Delete(item.FilePath);
            }
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
                if (item.Value == ((sender as CheckBox).BindingContext as CheckBoxItem).Value)
                {
                    if ((sender as CheckBox).IsChecked && !app.userSettings.ChosenResourceLanguages.Contains(item.Value))
                    {
                        app.userSettings.ChosenResourceLanguages.Add(item.Value);
                        app.SaveUserSettings();
                        break;
                    }
                    else if (!(sender as CheckBox).IsChecked && app.userSettings.ChosenResourceLanguages.Contains(item.Value))
                    {
                        app.userSettings.ChosenResourceLanguages.Remove(item.Value);
                        app.SaveUserSettings();
                        break;
                    }
                }
            }
        }
    }
}
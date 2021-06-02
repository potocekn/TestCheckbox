using System.Collections.Generic;
using AppBaseNamespace.ViewModels;
using System.Linq;
using Xamarin.Forms;
using AppBase.ViewModels;
using AppBase.Resources;
using AppBase.Helpers;
using System.IO;
using AppBase.UserSettingsHelpers;
using System;
using System.Globalization;

namespace AppBaseNamespace
{
    /// <summary>
    /// Class representing view model for resource format settings page. The model remembers all switches and all languages of the resources.
    /// </summary>
    internal class ResourceFormatSettingsPageViewModel
    {
        public List<ResourceFormatSettingsItem> Switches { get; }
        public List<LanguageSettingsItem> Languages { get; set; }
        App app;
        MainPageViewModel mainPageViewModel;
        public ResourceFormatSettingsPageViewModel(App app, MainPageViewModel mainPageViewModel, List<string> languages, List<ResourceFormatSettingsItem> switches)
        {
            this.app = app;
            this.mainPageViewModel = mainPageViewModel;
            
            Languages = languages
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new LanguageSettingsItem()
                {
                    IsChecked = app.userSettings.ChosenResourceLanguages.Contains(x),
                    EnglishName = x,
                    Value = LanguagesTranslationHelper.ReturnTranslation(x),
                    WasUpdated = false, 
                    Shortcut = GetLanguageShortcut(x)
                })
                .ToList();
            Switches = switches;            
        }

        string GetLanguageShortcut(string languageName)
        {
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (var item in cultures)
            {
                if (item.EnglishName == languageName)
                {
                    return item.TwoLetterISOLanguageName;
                }
            }
            return null;
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
        
        void DeleteUntoggledFormats()
        {
            if (!app.userSettings.Formats.Contains("PDF"))
            {
                RemoveFiles(ref app.resourcesPDF);
            }

            if (!app.userSettings.Formats.Contains("ODT"))
            {
                RemoveFiles(ref app.resourcesODT);
            }

            if (!app.userSettings.Formats.Contains("HTML"))
            {
                RemoveHTMLs();
            }
        }

        internal async void RequestUpdate(ResourceFormatSettingsPage page)
        {
            DeleteUntoggledFormats();
            DeleteUncheckedLanguageFiles();
            bool result = await UpdateSyncHelpers.DownloadResources(app);
            if (result)
            {
                await page.DisplayAlert(AppResources.ResourcesDownloadedTitle_Text, AppResources.ResourcesDownloadedMessage_Text, "OK");
                app.ReloadApp();
            }
            else
            {
                await page.DisplayAlert(AppResources.ResourcesDownloadedTitle_Text, AppResources.ResourcesDownloadedUnsuccessful_Text, "OK");
            }
            
        }

        private void DeleteUncheckedLanguageFiles()
        {
            foreach (var item in Languages)
            {
                if (!app.userSettings.ChosenResourceLanguages.Contains(item.EnglishName))
                {
                    RemoveFiles(item.Shortcut);
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

        void RemoveHTMLs()
        {
            var records = App.Database.GetPagesAsync().Result;
            foreach (var item in records)
            {
                App.Database.DeletePageAsync(item);
            }
        }

        void RemoveHTMLs(string language)
        {
            var records = App.Database.GetPagesAsync().Result;
            foreach (var item in records)
            {
                if (item.PageLanguage == language)
                {
                    App.Database.DeletePageAsync(item);
                }
            }
        }

        void RemoveFiles(ref List<ResourcesInfoPDF> list)
        {
            if (list == null)
                return;
            foreach (var item in list)
            {
                File.Delete(item.FilePath);
            }
            list = new List<ResourcesInfoPDF>();
        }

        void RemoveFiles(string language, List<ResourcesInfoPDF> list)
        {
            if (list == null) return;
            List<ResourcesInfoPDF> toBeDeleted = new List<ResourcesInfoPDF>();
            CultureInfo ci = new CultureInfo(language);
            foreach (var item in list)
            {
                if (item.Language == ci.EnglishName)
                {
                    File.Delete(item.FilePath);
                    toBeDeleted.Add(item);
                }
            }

            DeleteFromList(toBeDeleted, list);
        }

        void DeleteFromList(List<ResourcesInfoPDF> whatToDelete, List<ResourcesInfoPDF> fromWhere)
        {
            foreach (var item in whatToDelete)
            {
                fromWhere.Remove(item);
            }
        }

        void RemoveFiles(string language)
        {
            RemoveHTMLs(language);
            RemoveFiles(language, app.resourcesPDF);
            RemoveFiles(language, app.resourcesODT);
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
                        //UpdateSyncHelpers.DownloadResources(app);
                        app.SaveUserSettings();
                        break;
                    }
                    else if (!(sender as CheckBox).IsChecked && app.userSettings.ChosenResourceLanguages.Contains(item.EnglishName))
                    {
                        app.userSettings.ChosenResourceLanguages.Remove(item.EnglishName);
                        //RemoveFiles(item.EnglishName);
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
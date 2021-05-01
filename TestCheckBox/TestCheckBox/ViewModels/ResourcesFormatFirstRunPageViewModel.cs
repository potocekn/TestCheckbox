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
    /// Class that represents view model for the format of resources page that is displayed on the very first run of the application.
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
                if (app.userSettings.Formats.Count == 0)
                {
                    app.userSettings.Formats.Add("HTML");
                }
                app.DownloadTestFiles();
                string shortcut = GetShortcut(app.userSettings.AppLanguage);
                CultureInfo language = new CultureInfo(shortcut);
                Thread.CurrentThread.CurrentUICulture = language;
                CultureInfo.CurrentUICulture = language;
                AppResources.Culture = language;
                app.ReloadApp(shortcut, app.userSettings.AppLanguage);
                                
            });
        }

        /// <summary>
        /// Get the shortcut from the language full name. If there is no shortcut available empty string is returned.
        /// </summary>
        /// <param name="language">full name of the language</param>
        /// <returns>shortcut of the language</returns>
        string GetShortcut(string language)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            foreach (var culture in cultures)
            {
                if (culture.EnglishName == language) return culture.TwoLetterISOLanguageName;
            }
            return "";
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
                    HandleFormatChange(item.Name, (sender as Switch).IsToggled);                    
                }
            }
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

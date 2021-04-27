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
                    app.userSettings.Formats.Add("PDF");
                }
                string shortcut = GetShortcut(app.userSettings.AppLanguage);
                CultureInfo language = new CultureInfo(shortcut);
                Thread.CurrentThread.CurrentUICulture = language;
                CultureInfo.CurrentUICulture = language;
                AppResources.Culture = language;
                app.ReloadApp(shortcut, app.userSettings.AppLanguage);
                                
            });
        }

        string GetShortcut(string language)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            foreach (var culture in cultures)
            {
                if (culture.EnglishName == language) return culture.TwoLetterISOLanguageName;
            }
            return "";
        }

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

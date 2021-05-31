using AppBase.Helpers;
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
    class FirstRunDownloadResourcesPageViewModel
    {  
        public FirstRunDownloadResourcesPageViewModel()
        {
                        
        }

        public async void Download(App app)
        {            
            await UpdateSyncHelpers.DownloadResources(app);
            UpdateApp(app);
        }

        void UpdateApp(App app)
        {
            string shortcut = GetShortcut(app.userSettings.AppLanguage);
            CultureInfo language = new CultureInfo(shortcut);
            Thread.CurrentThread.CurrentUICulture = language;
            CultureInfo.CurrentUICulture = language;
            AppResources.Culture = language;
            app.ReloadApp(shortcut, app.userSettings.AppLanguage);
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
    }
}

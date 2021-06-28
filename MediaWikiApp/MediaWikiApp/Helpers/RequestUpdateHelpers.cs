using AppBase.Resources;
using AppBase.UserSettingsHelpers;
using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using AppBaseNamespace.Models;
using System.Globalization;
using System.Threading.Tasks;
using AppBase.Interfaces;
using Xamarin.Essentials;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions;
using AppBase.PopUpPages;

namespace AppBase.Helpers
{
    public static class RequestUpdateHelpers
    {

        /// <summary>
        /// Method that checks if the device is using the wi-fi connection. This method is used for 
        /// checking if user who requested download only with wi-fi can download updates.
        /// </summary>
        /// <returns>Boolean representing if the wi-fi is on.</returns>
        static bool CanDownloadOnlyWithWifi()
        {            
            var profiles = Connectivity.ConnectionProfiles;
            if (profiles.Contains(ConnectionProfile.WiFi))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// Method for requesting update of the resources. 
        /// The update deletes the unselected formats and languages and downloads the newly
        /// selected language resources.
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="app">Reference to the current application instance</param>
        /// <param name="languages">Languages for which to request update</param>
        /// <returns></returns>
        public static async Task RequestUpdate(ContentPage page, App app, List<LanguageSettingsItem> languages)
        {
            if (app.userSettings.DownloadOnlyWithWifi)
            {
                bool canDownload = CanDownloadOnlyWithWifi();
                if (!canDownload)
                {
                    await ShowPopupHelpers.ShowOKPopup(page, AppResources.ResourcesDownloadedTitle_Text, AppResources.DownloadOnlyWithWifi_Text, 300, 250);
                    return;
                }
            }

            await ShowPopupHelpers.ShowOKPopup(page, AppResources.ResourcesDownloadStartTitle_Text, AppResources.ResourcesDownloadStartMessage_Text, 300, 250);
            
            DeleteUntoggledFormats(app);
            DeleteUncheckedLanguageFiles(app, languages);
            bool result = await UpdateSyncHelpers.DownloadResources(app);
            if (result)
            {
                await ShowPopupHelpers.ShowOKPopup(page, AppResources.ResourcesDownloadedTitle_Text, AppResources.ResourcesDownloadedMessage_Text, 300, 180);
                app.ReloadApp();
            }
            else
            {
                await ShowPopupHelpers.ShowOKPopup(page, AppResources.ResourcesDownloadedTitle_Text, AppResources.ResourcesDownloadedUnsuccessful_Text, 300, 250);
            }

        }

        /// <summary>
        /// Method for deleting the unselected languages. All files and database records are deleted for these languages.
        /// </summary>
        /// <param name="app">Reference to the current application instance</param>
        /// <param name="languages">Languages to delete</param>
        private static void DeleteUncheckedLanguageFiles(App app, List<LanguageSettingsItem> languages)
        {
            foreach (var item in languages)
            {
                if (!app.userSettings.ChosenResourceLanguages.Contains(item.EnglishName))
                {
                    RemoveFiles(item.Shortcut, app);
                }
            }
        }

        /// <summary>
        /// Method for removing HTML files for given language.
        /// </summary>
        /// <param name="language">The language for which to delete.</param>
        static void RemoveHTMLs(string language)
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

        /// <summary>
        /// Method for deleting PDF or ODT files for specified language.
        /// </summary>
        /// <param name="language">Language for which to delete.</param>
        /// <param name="list">List of files to delete.</param>
        static void RemoveFiles(string language, List<ResourcesInfoPDF> list)
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

        /// <summary>
        /// Method for deleting the subset of resource list.
        /// </summary>
        /// <param name="whatToDelete">files to delete.</param>
        /// <param name="fromWhere">from what list to delete.</param>
        static void DeleteFromList(List<ResourcesInfoPDF> whatToDelete, List<ResourcesInfoPDF> fromWhere)
        {
            foreach (var item in whatToDelete)
            {
                fromWhere.Remove(item);
            }
        }

        /// <summary>
        /// Method for removing all files and formats of given language.
        /// </summary>
        /// <param name="language">The language for which language to delete.</param>
        /// <param name="app">Reference to the current application instance</param>
        static void RemoveFiles(string language, App app)
        {
            RemoveHTMLs(language);
            RemoveFiles(language, app.resourcesPDF);
            RemoveFiles(language, app.resourcesODT);
        }

        /// <summary>
        /// Method for deleting the formats that are no longer selected.
        /// </summary>        
        /// <param name="app">Reference to the current application instance</param>
        static void DeleteUntoggledFormats(App app)
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

        /// <summary>
        /// Method for removing all of the HTML records.
        /// </summary>
        static void RemoveHTMLs()
        {
            var records = App.Database.GetPagesAsync().Result;
            foreach (var item in records)
            {
                App.Database.DeletePageAsync(item);
            }
        }

        /// <summary>
        /// Method for removing the PDF or ODT files.
        /// </summary>
        /// <param name="list">list of files to delete.</param>
        static void RemoveFiles(ref List<ResourcesInfoPDF> list)
        {
            if (list == null)
                return;
            foreach (var item in list)
            {
                File.Delete(item.FilePath);
            }
            list = new List<ResourcesInfoPDF>();
        }
    }
}

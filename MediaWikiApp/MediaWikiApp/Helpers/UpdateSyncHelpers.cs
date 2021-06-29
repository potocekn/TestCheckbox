using AppBase.Interfaces;
using AppBase.Models;
using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppBase.Helpers
{
    /// <summary>
    /// Class that contains methods for ensuring synchronization of resources with the github server.
    /// </summary>
    static class UpdateSyncHelpers
    {
        static Dictionary<string, List<ChangesItem>> changes = new Dictionary<string, List<ChangesItem>>();
        static Dictionary<string, List<string>> languagesWithResources = new Dictionary<string, List<string>>();

        /// <summary>
        /// Application responsible for choosing the correct form of update strategy based on the user settings saved in the application.
        /// The default strategy is the automatic update.
        /// </summary>
        /// <param name="app">Reference to the current application. This is needed so that after the synchronization the database and 
        /// lists of resources in the app would be correctly updated and saved. </param>
        public static async void SynchronizeResources(App app)
        {
            switch (app.userSettings.UpdateInterval)
            {
                case UpdateIntervalOption.AUTOMATIC:
                    await HandleAutomaticUpdate(DateTime.Now, app);
                    break;
                case UpdateIntervalOption.ONCE_A_MONTH:
                    HandleOnceAMonthUpdate(DateTime.Now, app);
                    break;
                case UpdateIntervalOption.ON_REQUEST:
                    HandleOnRequestUpdate(DateTime.Now, app);
                    break;
                default:
                    await HandleAutomaticUpdate(DateTime.Now, app);
                    break;
            }
        }

        /// <summary>
        /// Wrapper function for requesting update. The wrapper was made because the update method should not be called uncontrolled.
        /// </summary>
        /// <param name="app">Reference to the current application. This is needed so that after the synchronization the database and 
        /// lists of resources in the app would be correctly updated and saved. </param>
        /// <returns>Boolean that represents if the update was successful. True => successful, False => not successful.</returns>
        public static async Task<bool> DownloadResources(App app)
        {
           return await HandleAutomaticUpdate(DateTime.Now, app);
        }

        /// <summary>
        /// Method for handling the "On request" update strategy.
        /// </summary>
        /// <param name="now">the actual date and time</param>
        /// <param name="app">Reference to the current application. This is needed so that after the synchronization the database and 
        /// lists of resources in the app would be correctly updated and saved. </param>
        private static async void HandleOnRequestUpdate(DateTime now, App app)
        {
            await HandleAutomaticUpdate(now, app);
        }

        /// <summary>
        /// Method for handling the "Once a month" update strategy. Update is done after at least 28 days.
        /// </summary>
        /// <param name="now">the actual date and time</param>
        /// <param name="app">Reference to the current application. This is needed so that after the synchronization the database and 
        /// lists of resources in the app would be correctly updated and saved. </param>
        private static async void HandleOnceAMonthUpdate(DateTime now, App app)
        {
            if (now.Subtract(app.userSettings.DateOfLastUpdate).TotalDays > 28)
            {
                await HandleAutomaticUpdate(now, app);
            }
        }

        /// <summary>
        /// Method for handling the "Automatic" update strategy. Update consists of synchronizing with the guthub repository and 
        /// downloading the changed or new resources. 
        /// </summary>
        /// <param name="now">the actual date and time</param>
        /// <param name="app">Reference to the current application. This is needed so that after the synchronization the database and 
        /// lists of resources in the app would be correctly updated and saved. </param>
        /// <returns>Boolean that represents if the update was successful. True => successful, False => not successful.</returns>
        private static async Task<bool> HandleAutomaticUpdate(DateTime now, App app)
        {
            if (!CanDownload(app)) return false;
            app.userSettings.DateOfLastUpdate = now;
            app.availableLanguages = DownloadLanguages(app.URL);
            languagesWithResources = DownloadLanguagesWithResources(app.URL);
            foreach (var language in languagesWithResources.Keys)
            {
                CultureInfo ci = new CultureInfo(language);
                List<ChangesItem> language_changes = DownloadChangedFiles(app.URL + '/' + language);
                if (changes.Keys.Contains(language))
                {
                    changes[language] = language_changes;
                }
                else
                {
                    changes.Add(language, language_changes);
                }                
            }

            bool result = true;

            foreach (var format in app.userSettings.Formats)
            {
                
                switch (format)
                {
                    case "HTML":
                        result &= await DownloadHTMLFiles(app);                    
                        break;
                    case "PDF":
                        result &= DownloadSpecialFormatFiles(app, app.resourcesPDF, ".pdf", "PDF");                       
                        break;                    
                    case "ODT":
                        result &= DownloadSpecialFormatFiles(app, app.resourcesODT, ".odt", "ODT");                        
                        break;
                    default:                       
                        break;
                }
            }
           return result;
        }

        /// <summary>
        /// Method for downloading the PDF or ODT files from the github repository.
        /// </summary>
        /// <param name="app">Reference to the current application. This is needed so that after the synchronization the database and 
        /// lists of resources in the app would be correctly updated and saved. </param>
        /// <param name="list">List of current resources of given format in the application.</param>
        /// <param name="fileFormat">Format of the files that should be downloaded/updated. (".pdf for PDF files and ".odt" for ODT files)</param>
        /// <param name="formatFolder">The name of the folder in the repository where the resources are stored ("PDF" or "ODT").</param>
        /// <returns>Boolean that represents if the update was successful. True => successful, False => not successful.</returns>
        private static bool DownloadSpecialFormatFiles(App app, List<ResourcesInfo> list, string fileFormat, string formatFolder)
        {
            if (!CanDownload(app)) return false;

            if (languagesWithResources == null || languagesWithResources.Count == 0)
                return false;

            try
            {
                IDownloader downloader = DependencyService.Get<IDownloader>();
                foreach (var language in languagesWithResources.Keys)
                {
                    CultureInfo ci = new CultureInfo(language);
                    if (!app.userSettings.ChosenResourceLanguages.Contains(ci.DisplayName))
                    {
                        continue;
                    }
                    string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ci.DisplayName);

                    foreach (var resource in languagesWithResources[language])
                    {
                        if (list == null)
                        {
                            list = new List<ResourcesInfo>();
                        }
                        var found = list.Find(x => x.FileName.Contains(resource) && x.FileName.Contains(language));
                        ResourcesInfo newResource;
                        if (found == null)
                        {
                            newResource = new ResourcesInfo()
                            {
                                FileName = resource + "-" + language + fileFormat,
                                Language = ci.DisplayName,
                                ResourceName = resource + "-" + language,
                                FilePath = Path.Combine(dir, resource + "-" + language + fileFormat),
                                Url = app.URL + "/" + language + "/" + formatFolder + "/" + resource + "-" + language + fileFormat
                            };
                            list.Add(newResource);
                        }
                        else
                        {
                            newResource = found;
                        }

                        bool changesContainResource = false;
                        if (changes.Keys.Contains(language))
                        {
                            foreach (var change in changes[language])
                            {
                                if (change.FileName.Contains(resource))
                                {
                                    if (change.VersionNumber > newResource.Version)
                                    {
                                        changesContainResource = true;
                                        newResource.Version = change.VersionNumber;
                                        downloader.DownloadFile(newResource.Url, dir, newResource.FileName);
                                    }
                                }
                            }
                        }

                        if (!changesContainResource)
                        {
                            newResource.Version = 1;
                            downloader.DownloadFile(newResource.Url, dir, newResource.FileName);
                        }

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }            
        }

        /// <summary>
        /// Method used for downloading the dictionary that contains the shortcut of the language as a key 
        /// and as values the names of all resources available for given language. 
        /// This file is stored in the main github repository, in the root folder as "LanguagesWithResources.json" 
        /// </summary>
        /// <param name="url">The url of the github repository - the root folder, master branch.</param>
        /// <returns>Dictionary of languages and their available resources.</returns>
        private static Dictionary<string, List<string>> DownloadLanguagesWithResources(string url)
        {
            string contents = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    contents = client.GetStringAsync(url + "/LanguagesWithResources.json").GetAwaiter().GetResult();
                }

                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(contents);
            }
            catch
            {
                return new Dictionary<string, List<string>>();
            }            
        }

        /// <summary>
        /// Method for downloading a list of all available languages from the github repository. All languages available on the mediawiki server are
        /// stored in the root folder of the github repository in the "Languages.json" file.
        /// </summary>
        /// <param name="url">The url of the github repository - the root folder, master branch.</param>
        /// <returns>List of all available languages on the mediawiki server.</returns>
        public static List<string> DownloadLanguages(string url)
        {
            try
            {
                string contents = "";
                using (WebClient client = new WebClient())
                {
                    contents = client.DownloadString(url + "/Languages.json");
                }

                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(contents);
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Method for downloading a list of all changed files for a specific language. Every resource has a version number. The tuple of 
        /// (resource name, version number) is stored in the "Changes.json" file in the specific language folder in the github repository.
        /// </summary>
        /// <param name="url">The url of the github repository + the language folder.</param>
        /// <returns>The list of resources and their actual version numbers.</returns>
        private static List<ChangesItem> DownloadChangedFiles(string url)
        {            
            string contents = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    contents = client.GetStringAsync(url + "/Changes.json").GetAwaiter().GetResult();
                }

                var contentsDeserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<object>>>(contents);
                var result = ParseChangesResults(contentsDeserialized);                
                return result;
            }
            catch 
            {
                return new List<ChangesItem>() ;
            }            
        }

        /// <summary>
        /// Method for creating ChangesItems from deserialized json file. The version number is converted from string to int. 
        /// The method expects the correct format of the version number.
        /// </summary>
        /// <param name="changes">Deserialized json result from the Changes.json file in the language folder.</param>
        /// <returns>The list of resources and their actual version numbers.</returns>
        private static  List<ChangesItem> ParseChangesResults(List<List<object>> changes)
        {
            if (changes == null) return null;
            List<ChangesItem> result = new List<ChangesItem>();

            foreach (var change in changes)
            {
                string fileName = (string)change[0];                
                int versionNumber = Convert.ToInt32(change[1]);
                
                ChangesItem newItem = new ChangesItem()
                {
                    FileName = fileName,
                    VersionNumber = versionNumber
                };
                result.Add(newItem);
            }

            return result;
        }

        /// <summary>
        /// Method used for determining if conditions for downloading are satisfied - internet access + if user 
        /// requested to download only when wifi is on, then the type of connection has to be wifi.
        /// </summary>
        /// <param name="app">Reference to the current app.</param>
        /// <returns>If it is possible to start download.</returns>
        public static bool CanDownload(App app)
        {
            bool canDownload = false;
            if (app.userSettings.DownloadOnlyWithWifi)
            {
                var profiles = Connectivity.ConnectionProfiles;
                if (profiles.Contains(ConnectionProfile.WiFi))
                {
                    canDownload = true;
                }
            }
            else
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    canDownload = true;
                }
            }
            return canDownload;
        }

        /// <summary>
        /// Method for downloading and saving the changed files. When the version of resource is higher that the app remembers, 
        /// the new version has to be downloaded and has to replace the older version.
        /// </summary>
        /// <param name="changes">List of items that are changed.</param>
        /// <param name="language">For which language the update should be done (shortcut).</param>
        /// <param name="URL">URL to the folder where the resource files are located.</param>
        /// <param name="wc">the Web client used for downloading.</param>
        /// <returns>Boolean if the operation was successful.</returns>
        static async Task<bool> SaveChanges(List<ChangesItem> changes, string language, string URL, WebClient wc)
        {
            try
            {
                foreach (var change in changes)
                {
                    var resourceName = change.FileName.Split('/', '.')[2];
                    var existingDbsRecord = App.Database.GetPageAsync(resourceName + "-" + language).Result;
                    string fullRecordURL = URL + "/" + change.FileName;

                    if (existingDbsRecord == null)
                    {
                        string contents = wc.DownloadString(fullRecordURL);
                        HtmlRecord record = new HtmlRecord
                        {
                            PageContent = contents,
                            PageName = resourceName + "-" + language,
                            PageLanguage = language
                        };

                        await App.Database.SavePageAsync(record);
                    }
                    else
                    {
                        if (change.VersionNumber > existingDbsRecord.VersionNumber)
                        {
                            string contents = wc.DownloadString(fullRecordURL);
                            existingDbsRecord.PageContent = contents;
                            existingDbsRecord.VersionNumber = change.VersionNumber;
                            await App.Database.SavePageAsync(existingDbsRecord);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
           
        }

        /// <summary>
        /// Method for downloading the HTML files for each language that user selected.
        /// </summary>
        /// <param name="app">The reference to the current app so that the Database can be properly updated.</param>
        /// <returns>Boolean if the operation was successful.</returns>
        public static async Task<bool> DownloadHTMLFiles(App app)
        { 
            if (!CanDownload(app)) return false;
                        
            if (languagesWithResources == null) return false;

            foreach (var item in app.userSettings.ChosenResourceLanguages)
            {                              
                try
                {
                    using (var wc = new System.Net.WebClient())
                    {
                        //key == language, value == name                    
                        foreach (var language in languagesWithResources.Keys)
                        {
                            CultureInfo ci = new CultureInfo(language);
                            List<ChangesItem> language_changes;
                            bool success = changes.TryGetValue(language, out language_changes);
                            if (success)
                            {
                                if (ci.DisplayName == item)
                                {
                                    bool result = await SaveChanges(language_changes, language, app.URL, wc);
                                }
                            }                            
                        }
                    }                    
                }
                catch 
                {
                    return false ;
                }
                
            }
            return true;
        }       
    }
}

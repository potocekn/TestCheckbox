using AppBase.Interfaces;
using AppBase.Models;
using AppBase.UserSettingsHelpers;
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
    static class UpdateSyncHelpers
    {
        static Dictionary<string, List<ChangesItem>> changes = new Dictionary<string, List<ChangesItem>>();
        static Dictionary<string, List<string>> languagesWithResources = new Dictionary<string, List<string>>();
        public static async void SynchronizeResources(App app)
        {
            switch (app.userSettings.UpdateInterval)
            {
                case "Automatic":
                    await HandleAutomaticUpdate(DateTime.Now, app);
                    break;
                case "Once a Month":
                    HandleOnceAMonthUpdate(DateTime.Now, app);
                    break;
                case "On request":
                    HandleOnRequestUpdate(DateTime.Now, app);
                    break;
                default:
                    await HandleAutomaticUpdate(DateTime.Now, app);
                    break;
            }
        }

        public static async Task<bool> DownloadResources(App app)
        {
           return await HandleAutomaticUpdate(DateTime.Now, app);
        }

        private static async void HandleOnRequestUpdate(DateTime now, App app)
        {
            await HandleAutomaticUpdate(now, app);
        }

        private static async void HandleOnceAMonthUpdate(DateTime now, App app)
        {
            if (now.Subtract(app.userSettings.DateOfLastUpdate).TotalDays > 28)
            {
                await HandleAutomaticUpdate(now, app);
            }
        }

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
                        //return true; //for now
                        break;                    
                    case "ODT":
                        result &= DownloadSpecialFormatFiles(app, app.resourcesODT, ".odt", "ODT");
                        //return true; //for now
                        break;
                    default:
                        //return false;
                        break;
                }
            }
           return result;
        }

        private static bool DownloadSpecialFormatFiles(App app, List<ResourcesInfoPDF> list, string fileFormat, string formatFolder)
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
                            list = new List<ResourcesInfoPDF>();
                        }
                        var found = list.Find(x => x.FileName.Contains(resource) && x.FileName.Contains(language));
                        ResourcesInfoPDF newResource;
                        if (found == null)
                        {
                            newResource = new ResourcesInfoPDF()
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
                return null;
            }            
        }

        public static List<string> DownloadLanguages(string url)
        {
            string contents = "";
            using (WebClient client = new WebClient())
            {
                contents = client.DownloadString(url + "/Languages.json");
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(contents);
        }

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
                return null;
            }            
        }


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

        public static async void DownloadTestFiles(App app)
        {
            if (!CanDownload(app)) return;
            app.resourcesPDF = new List<ResourcesInfoPDF>();
            app.resourcesODT = new List<ResourcesInfoPDF>();
            IDownloader downloader = DependencyService.Get<IDownloader>();
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "English");
            string fileName = Path.Combine(dir, "test.pdf");
            ResourcesInfoPDF item = new ResourcesInfoPDF()
            {
                Language = "English",
                ResourceName = "Test Resource",
                FileName = "test.pdf",
                Url = "http://www.4training.net/mediawiki/images/a/af/Gods_Story_%28five_fingers%29.pdf",
                FilePath = fileName
            };
            app.resourcesPDF.Add(item);

            fileName = Path.Combine(dir, "test2.pdf");
            ResourcesInfoPDF item2 = new ResourcesInfoPDF()
            {
                Language = "English",
                ResourceName = "Test Resource 2",
                FileName = "test2.pdf",
                Url = "http://www.4training.net/mediawiki/images/8/8b/Baptism.pdf",
                FilePath = fileName
            };
            app.resourcesPDF.Add(item2);

            foreach (var pdf in app.resourcesPDF)
            {
                string save_dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), pdf.Language);
                try
                {
                    downloader.DownloadFile(pdf.Url, save_dir, pdf.FileName);
                }
                catch (Exception e)
                {
                    
                }
            }

            fileName = Path.Combine(dir, "test3.odt");
            ResourcesInfoPDF item3 = new ResourcesInfoPDF()
            {
                Language = "English",
                ResourceName = "Test Resource ODT",
                FileName = "test3.odt",
                Url = "https://www.4training.net/mediawiki/images/a/a8/Church.odt",
                FilePath = fileName
            };
            app.resourcesODT.Add(item3);

            foreach (var odt in app.resourcesODT)
            {
                string save_dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), odt.Language);
                downloader.DownloadFile(odt.Url, save_dir, odt.FileName);
            }

            var downloadedImage = await ImageService.DownloadImage("https://www.4training.net/mediawiki/images/3/3b/Relationship_Triangle.png");

            ImageService.SaveToDisk("testImage.png", downloadedImage);
        }
        
    }
}

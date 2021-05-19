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
            app.userSettings.DateOfLastUpdate = now;
            foreach (var format in app.userSettings.Formats)
            {
                switch (format)
                {
                    case "PDF":
                        DownloadTestFiles(app);
                        //return true; //for now
                        break;
                    case "HTML":
                        return await DownloadHTMLFiles(app);
                        //return result.Result;
                        //break;
                    case "ODT":
                        DownloadTestFiles(app);
                        //return true; //for now
                        break;
                    default:
                        //return false;
                        break;
                }
            }
           return false;
        }

        private static void DownloadPDFFiles(App app)
        {
            IDownloader downloader = DependencyService.Get<IDownloader>();
            foreach (var item in app.resourcesPDF)
            {
                string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), item.Language);
                downloader.DownloadFile(item.Url, dir, item.FileName);
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
            using (HttpClient client = new HttpClient())
            {
                contents = client.GetStringAsync(url + "/Languages.json").GetAwaiter().GetResult();
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


        private static bool CanDownload(App app)
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

        static async Task<bool> Test(List<ChangesItem> changes, string language, string URL, WebClient wc)
        {
            try
            {
                foreach (var change in changes)
                {
                    var resourceName = change.FileName.Split('/', '.')[1];
                    var existingDbsRecord = App.Database.GetPageAsync(resourceName + "(" + language + ")").Result;
                    string fullRecordURL = URL + "/" + change.FileName;

                    if (existingDbsRecord == null)
                    {
                        string contents = wc.DownloadString(fullRecordURL);
                        HtmlRecord record = new HtmlRecord
                        {
                            PageContent = contents,
                            PageName = resourceName + "(" + language + ")",
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

            Dictionary<string, List<string>> languagesWithResources = DownloadLanguagesWithResources(app.URL);
            if (languagesWithResources == null) return false;

            foreach (var item in app.userSettings.ChosenResourceLanguages)
            {               
                //string contents;

                try
                {
                    using (var wc = new System.Net.WebClient())
                    {
                        //key == language, value == name                    
                        foreach (var language in languagesWithResources.Keys)
                        {
                            List<ChangesItem> changes = DownloadChangedFiles(app.URL + '/' + language);
                            CultureInfo ci = new CultureInfo(language);
                            if (ci.DisplayName == item)
                            {
                                bool result = await Test(changes, item, app.URL, wc);
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

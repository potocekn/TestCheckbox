﻿using AppBase.Interfaces;
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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppBase.Helpers
{
    static class UpdateSyncHelpers
    {
        public static void SynchronizeResources(App app)
        {
            switch (app.userSettings.UpdateInterval)
            {
                case "Automatic":
                    HandleAutomaticUpdate(DateTime.Now, app);
                    break;
                case "Once a Month":
                    HandleOnceAMonthUpdate(DateTime.Now, app);
                    break;
                case "On request":
                    break;
                default:
                    HandleAutomaticUpdate(DateTime.Now, app);
                    break;
            }
        }

        public static void DownloadResources(App app)
        {
            HandleAutomaticUpdate(DateTime.Now, app);
        }

        private static void HandleOnRequestUpdate(DateTime now, App app)
        {
            HandleAutomaticUpdate(now, app);
        }

        private static void HandleOnceAMonthUpdate(DateTime now, App app)
        {
            if (now.Subtract(app.userSettings.DateOfLastUpdate).TotalDays > 28)
            {
                HandleAutomaticUpdate(now, app);
            }
        }

        private static void HandleAutomaticUpdate(DateTime now, App app)
        {
            app.userSettings.DateOfLastUpdate = now;
            foreach (var format in app.userSettings.Formats)
            {
                switch (format)
                {
                    case "PDF":
                        DownloadTestFiles(app);
                        break;
                    case "HTML":
                        DownloadHTMLFiles(app);
                        break;
                    case "ODT":
                        DownloadTestFiles(app);
                        break;
                    default:
                        break;
                }
            }
            
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
            using (HttpClient client = new HttpClient())
            {
                contents = client.GetStringAsync(url + "/LanguagesWithResources.json").GetAwaiter().GetResult();
            }
            
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(contents);
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
            using (HttpClient client = new HttpClient())
            {
                contents = client.GetStringAsync(url + "/Changes.json").GetAwaiter().GetResult();
            }

            var contentsDeserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<object>>>(contents);
            var result = ParseChangesResults(contentsDeserialized);
            return result; 
        }


        private static  List<ChangesItem> ParseChangesResults(List<List<object>> changes)
        {
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

        public static async void DownloadHTMLFiles(App app)
        { 
            if (!CanDownload(app)) return;

            Dictionary<string, List<string>> languagesWithResources = DownloadLanguagesWithResources(app.URL);            

            foreach (var item in app.userSettings.ChosenResourceLanguages)
            {               
                string contents;
                using (var wc = new System.Net.WebClient())
                {
                    //key == language, value == name
                    
                    foreach (var language in languagesWithResources.Keys)
                    {
                        List<ChangesItem> changes = DownloadChangedFiles(app.URL + '/' + language);
                        CultureInfo ci = new CultureInfo(language);
                        if (ci.DisplayName == item)
                        {
                            foreach (var change in changes)
                            {
                                var resourceName = change.FileName.Split('/', '.')[1];
                                var existingDbsRecord = App.Database.GetPageAsync(resourceName + "(" + item + ")").Result;
                                string fullRecordURL = app.URL + "/" + change.FileName;                                

                                if (existingDbsRecord == null)
                                {
                                    contents = wc.DownloadString(fullRecordURL);
                                    HtmlRecord record = new HtmlRecord
                                    {
                                        PageContent = contents,
                                        PageName = resourceName + "(" + item + ")",
                                        PageLanguage = item
                                    };

                                    await App.Database.SavePageAsync(record);
                                }
                                else
                                {
                                    if (change.VersionNumber > existingDbsRecord.VersionNumber)
                                    {
                                        contents = wc.DownloadString(fullRecordURL);
                                        existingDbsRecord.PageContent = contents;
                                        existingDbsRecord.VersionNumber = change.VersionNumber;
                                        await App.Database.SavePageAsync(existingDbsRecord);
                                    }                                                                                                                                          
                                }
                            }                            
                        }
                    }                    
                }
            }
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

using System;
using Xamarin.Forms;
using System.Globalization;
using Xamarin.Forms.Xaml;
using System.Threading;
using AppBase.Resources;
using System.IO;
using System.Collections.Generic;
using AppBase.UserSettingsHelpers;
using AppBase;
using AppBase.Interfaces;
using AppBase.Helpers;

namespace AppBaseNamespace
{
    public partial class App : Application
    {
        bool firstTimeRunning = true;
        string userSettingsfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "userSettings.json");
        string resourcesfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "resources.json");

        public bool IsFirst = true;
        public bool WasRefreshed = false;
        public UserSettings userSettings;
        public List<ResourcesInfo> resources;

        Dictionary<string, string> shortcuts = new Dictionary<string, string>();
        public App()
        {
            InitializeShortcuts();
            InitializeComponent();
            RetrieveUserSettings(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            RetrieveResources();
            SetAppLanguage(userSettings.AppLanguage);
            SynchronizeResources();
            if (firstTimeRunning)
            {
                MainPage = new NavigationPage(new AppLanguageFirstRunPage(this));
            }
            else
            {
                MainPage = new NavigationPage(new MainPage(this, userSettings.AppLanguage));
            }
            
        }

        private void InitializeShortcuts()
        {
            shortcuts.Add("English", "en");
            shortcuts.Add("German", "de");
            shortcuts.Add("Czech", "cs");
            shortcuts.Add("French", "fr");
            shortcuts.Add("Chinese", "zh-Hans");
        }
        private void SetAppLanguage(string languageName) 
        {              
            string shortcut;
            bool success = shortcuts.TryGetValue(languageName, out shortcut);
            CultureInfo language;
            if (success)
            {
                language = new CultureInfo(shortcut);
            }
            else
            {
                language = new CultureInfo("en");
            }

            Thread.CurrentThread.CurrentUICulture = language;
            AppResources.Culture = language; 
            
        }

        private async void RetrieveResources()
        {
            List<ResourcesInfo> resourcesInfos = new List<ResourcesInfo>();

            if (File.Exists(resourcesfileName))
            {
                resourcesInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResourcesInfo>>(File.ReadAllText(resourcesfileName).Trim());
            }

            resources = resourcesInfos;

            /////////////////////////
            ////    temporary   /////
            /////////////////////////
            resources = new List<ResourcesInfo>();

            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "English");
            string fileName = Path.Combine(dir, "test.pdf");
            ResourcesInfo item = new ResourcesInfo()
            {
                Language = "English",
                ResourceName = "Test Resource",
                FileName = "test.pdf",
                Url = "http://www.4training.net/mediawiki/images/a/af/Gods_Story_%28five_fingers%29.pdf",
                FilePath = fileName
            };
            resources.Add(item);

            fileName = Path.Combine(dir, "test2.pdf");
            ResourcesInfo item2 = new ResourcesInfo()
            {
                Language = "English",
                ResourceName = "Test Resource 2",
                FileName = "test2.pdf",
                Url = "http://www.4training.net/mediawiki/images/8/8b/Baptism.pdf",
                FilePath = fileName
            };
            resources.Add(item2);

            var downloadedImage = await ImageService.DownloadImage("https://www.4training.net/mediawiki/images/3/3b/Relationship_Triangle.png");

            ImageService.SaveToDisk("testImage.png", downloadedImage);
            /////////////////////////
            ////      end       /////
            /////////////////////////
        }

        private void RetrieveUserSettings(string path)
        {
            UserSettings result = new UserSettings(path);
            if (File.Exists(userSettingsfileName))
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText(userSettingsfileName).Trim());
                firstTimeRunning = false;
            }

            userSettings = result;
        }

        public void ReloadApp(string language, string previouslyChecked)
        {
            WasRefreshed = true;
            userSettings.AppLanguage = previouslyChecked;
            Application.Current.Properties["currentLanguage"] = language;
            File.WriteAllText(userSettingsfileName, Newtonsoft.Json.JsonConvert.SerializeObject(userSettings));
            MainPage = new NavigationPage(new MainPage(this, previouslyChecked));
        }

        void SynchronizeResources()
        {
            switch (userSettings.UpdateInterval)
            {
                case "Automatic":
                    HandleAutomaticUpdate(DateTime.Now);
                    break;
                case "Once a month":
                    HandleOnceAMonthUpdate(DateTime.Now);
                    break;
                case "On request":                    
                    break;
                default:
                    HandleAutomaticUpdate(DateTime.Now);
                    break;
            }
        }

        public  void HandleOnRequestUpdate(DateTime now)
        {
            //toto tu sa bude volat po kliknuti na button request update
        }

        private void HandleOnceAMonthUpdate(DateTime now)
        {           
            if (now.Subtract(userSettings.DateOfLastUpdate).TotalDays > 28)
            {                
                HandleAutomaticUpdate(now);
            }
        }

        private void HandleAutomaticUpdate(DateTime now)
        {
            userSettings.DateOfLastUpdate = now;
            //tuto sa bude so serverom komunikovat
        }

        public void SaveUserSettings()
        {
            File.WriteAllText(userSettingsfileName, Newtonsoft.Json.JsonConvert.SerializeObject(userSettings));
        }

        public void SaveResources()
        {
            File.WriteAllText(resourcesfileName, Newtonsoft.Json.JsonConvert.SerializeObject(resources));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            SaveUserSettings();
            SaveResources();
        }

        protected override void OnResume()
        {
        }
    }
}

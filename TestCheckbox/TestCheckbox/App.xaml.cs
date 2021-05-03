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
using Xamarin.Essentials;
using AppBase.Models;
using System.Linq;

namespace AppBaseNamespace
{
    public partial class App : Application
    {
        public bool firstTimeRunning = true;
        string userSettingsfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "userSettings.json");
        string resourcesPDFfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "resourcesPDF.json");
        string resourcesODTfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "resourcesODT.json");

        public bool IsFirst = true;
        public bool WasRefreshed = false;
        public string URL = "https://raw.githubusercontent.com/potocekn/ResourcesTest/master";
        public UserSettings userSettings;
        public List<ResourcesInfoPDF> resourcesPDF;
        public List<ResourcesInfoPDF> resourcesODT;
        public Dictionary<string, string> resourcesHTML;

        Dictionary<string, string> shortcuts = new Dictionary<string, string>();

        static HtmlDatabase database;

        // Create the database connection as a singleton.
        public static HtmlDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new HtmlDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HtmlPages.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeShortcuts();
            InitializeComponent();
            RetrieveUserSettings(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            if (!firstTimeRunning)
            {
                RetrieveResources();
                UpdateSyncHelpers.SaveHtmlToDbs();
                SetAppLanguage(userSettings.AppLanguage);
                UpdateSyncHelpers.SynchronizeResources(this);
            }
            
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
        private void RetrieveResources()
        {
            RetrieveResourcesPDF();
            RetrieveResourcesODT();
        }
        private void RetrieveResourcesPDF()
        {
            List<ResourcesInfoPDF> resourcesInfosPDF = new List<ResourcesInfoPDF>();

            if (File.Exists(resourcesPDFfileName))
            {
                resourcesInfosPDF = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResourcesInfoPDF>>(File.ReadAllText(resourcesPDFfileName).Trim());
            }

            resourcesPDF = resourcesInfosPDF;           
        }

        private void RetrieveResourcesODT()
        {
            List<ResourcesInfoPDF> resourcesInfosODT = new List<ResourcesInfoPDF>();

            if (File.Exists(resourcesODTfileName))
            {
                resourcesInfosODT = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResourcesInfoPDF>>(File.ReadAllText(resourcesODTfileName).Trim());
            }

            resourcesODT = resourcesInfosODT;
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

        public void SaveUserSettings()
        {
            File.WriteAllText(userSettingsfileName, Newtonsoft.Json.JsonConvert.SerializeObject(userSettings));
        }

        public void SaveResources()
        {
            File.WriteAllText(resourcesPDFfileName, Newtonsoft.Json.JsonConvert.SerializeObject(resourcesPDF));
            File.WriteAllText(resourcesODTfileName, Newtonsoft.Json.JsonConvert.SerializeObject(resourcesODT));
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

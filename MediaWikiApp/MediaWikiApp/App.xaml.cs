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
    /// <summary>
    /// Class that represents the application.
    /// </summary>
    public partial class App : Application
    {
        public bool firstTimeRunning = true;
        string userSettingsfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "userSettings.json");
        string resourcesPDFfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "resourcesPDF.json");
        string resourcesODTfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "resourcesODT.json");
        string languagesFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "languages.json");

        public bool IsFirst = true;        
        public bool WasRefreshed = false;
        public string URL = "https://raw.githubusercontent.com/potocekn/ResourcesTest/master";
        public UserSettings userSettings;
        public List<string> availableLanguages = new List<string>();
        public List<ResourcesInfoPDF> resourcesPDF = new List<ResourcesInfoPDF>();
        public List<ResourcesInfoPDF> resourcesODT = new List<ResourcesInfoPDF>();
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
            InitializeComponent();                        
        }

        /// <summary>
        /// Method that inicializes the list of languages and their shortcuts.
        /// </summary>
        private void InitializeShortcuts()
        {
            shortcuts.Add("English", "en");
            shortcuts.Add("German", "de");
            shortcuts.Add("Czech", "cs");
            shortcuts.Add("French", "fr");
            shortcuts.Add("Chinese", "zh-Hans");
        }

        /// <summary>
        /// Method that sets the application language.
        /// </summary>
        /// <param name="languageName">Name of the language that should be set.</param>
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

        /// <summary>
        /// Method that retrieves the saved PDF and ODt resources.
        /// </summary>
        private void RetrieveResources()
        {
            RetrieveResourcesPDF();
            RetrieveResourcesODT();
        }

        /// <summary>
        /// Method that retrieves the saved PDF files. 
        /// The method sets the list of resource information that this class has as an attribute.
        /// </summary>
        private void RetrieveResourcesPDF()
        {
            List<ResourcesInfoPDF> resourcesInfosPDF = new List<ResourcesInfoPDF>();

            if (File.Exists(resourcesPDFfileName))
            {
                resourcesInfosPDF = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResourcesInfoPDF>>(File.ReadAllText(resourcesPDFfileName).Trim());
            }

            resourcesPDF = resourcesInfosPDF;           
        }

        /// <summary>
        /// Method that retrieves the saved ODT files. 
        /// The method sets the list of resource information that this class has as an attribute.
        /// </summary>
        private void RetrieveResourcesODT()
        {
            List<ResourcesInfoPDF> resourcesInfosODT = new List<ResourcesInfoPDF>();

            if (File.Exists(resourcesODTfileName))
            {
                resourcesInfosODT = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResourcesInfoPDF>>(File.ReadAllText(resourcesODTfileName).Trim());
            }

            resourcesODT = resourcesInfosODT;
        }

        /// <summary>
        /// Method that retrieves the user settings.
        /// </summary>
        /// <param name="path">path to the user settings file</param>
        private void RetrieveUserSettings(string path)
        {
            UserSettings result = new UserSettings(path);
            if (File.Exists(userSettingsfileName))
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText(userSettingsfileName).Trim());                
            }

            userSettings = result;
        }

        /// <summary>
        /// Method that retrieves the list of resource languages.
        /// </summary>
        private void RetrieveLanguages()
        {
            List<string> result = new List<string>();
            if (File.Exists(languagesFileName))
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(languagesFileName).Trim());
                firstTimeRunning = false;
            }
            availableLanguages = result;
        }

        /// <summary>
        /// Method that reloads the application after the language change request.
        /// </summary>
        /// <param name="language">new language</param>
        /// <param name="previouslyChecked">previous language</param>
        public void ReloadApp(string language, string previouslyChecked)
        {
            WasRefreshed = true;
            userSettings.AppLanguage = previouslyChecked;
            Application.Current.Properties["currentLanguage"] = language;
            File.WriteAllText(userSettingsfileName, Newtonsoft.Json.JsonConvert.SerializeObject(userSettings));           
            var navPage = new NavigationPage(new MainPage(this, previouslyChecked));            
            navPage.BarBackgroundColor = Color.FromHex("#B3BAE4");
            MainPage = navPage;
        }

        /// <summary>
        /// Method that reloads the application.
        /// </summary>
        public void ReloadApp()
        {
            var navPage = new NavigationPage(new MainPage(this, userSettings.AppLanguage));
            navPage.BarBackgroundColor = Color.FromHex("#B3BAE4");
            MainPage = navPage;
        }

        /// <summary>
        /// Method that saves user settings into a JSON file.
        /// </summary>
        public void SaveUserSettings()
        {
            File.WriteAllText(userSettingsfileName, Newtonsoft.Json.JsonConvert.SerializeObject(userSettings));
        }

        /// <summary>
        /// Method that saves the resource languages into a JSON file.
        /// </summary>
        public void SaveLanguages()
        {
            if ((availableLanguages != null) && (availableLanguages.Count > 0))
                File.WriteAllText(languagesFileName, Newtonsoft.Json.JsonConvert.SerializeObject(availableLanguages));
        }

        /// <summary>
        /// Method that saves information about saved PDF and ODT resources into separate JSON files.
        /// </summary>
        public void SaveResources()
        {
            File.WriteAllText(resourcesPDFfileName, Newtonsoft.Json.JsonConvert.SerializeObject(resourcesPDF));
            File.WriteAllText(resourcesODTfileName, Newtonsoft.Json.JsonConvert.SerializeObject(resourcesODT));
        }

        /// <summary>
        /// Method that is called on every start of the application.
        /// </summary>
        protected override void OnStart()
        {           
            InitializeShortcuts();
            RetrieveUserSettings(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            RetrieveLanguages();
            if (!firstTimeRunning)
            {
                RetrieveResources();
                SetAppLanguage(userSettings.AppLanguage);
                UpdateSyncHelpers.SynchronizeResources(this);
                if (userSettings.WasFirstDownload)
                {
                    var navPage = new NavigationPage(new MainPage(this, userSettings.AppLanguage));
                    navPage.BarBackgroundColor = Color.FromHex("#B3BAE4");
                    MainPage = navPage;
                }
                else
                {
                    var navPage = new NavigationPage(new FirstRunDownloadResourcesPage(this));
                    navPage.BarBackgroundColor = Color.FromHex("#B3BAE4");
                    MainPage = navPage;
                }
            }
            else
            {
                var navPage = new NavigationPage(new AppLanguageFirstRunPage(this));
                navPage.BarBackgroundColor = Color.FromHex("#B3BAE4");
                MainPage = navPage;
            }
        }

        /// <summary>
        /// Method that is called when the application stops running.
        /// </summary>
        protected override void OnSleep()
        {
            SaveUserSettings();
            SaveResources();
            SaveLanguages();
        }

        protected override void OnResume()
        {
        }
    }
}

using System;
using Xamarin.Forms;
using System.Globalization;
using Xamarin.Forms.Xaml;
using System.Threading;
using AppBase.Resources;
using System.IO;
using System.Collections.Generic;
using AppBase.UserSettingsHelpers;

namespace AppBaseNamespace
{
    public partial class App : Application
    {
        public bool IsFirst = true;
        public bool WasRefreshed = false;
        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "userSettings.txt");
        public UserSettings userSettings;

        Dictionary<string, string> shortcuts = new Dictionary<string, string>();
        public App()
        {
            InitializeShortcuts();
            InitializeComponent();
            RetrieveUserSettings(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            SetAppLanguage(userSettings.AppLanguage);
            SynchronizeResources();
            MainPage = new NavigationPage(new MainPage(this, userSettings.AppLanguage));
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

        private void RetrieveUserSettings(string path)
        {
            UserSettings result = new UserSettings(path);
            if (File.Exists(fileName))
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText(fileName).Trim());
            }

            userSettings = result;
        }

        public void ReloadApp(string language, string previouslyChecked)
        {
            WasRefreshed = true;
            userSettings.AppLanguage = previouslyChecked;
            Application.Current.Properties["currentLanguage"] = language;
            File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(userSettings));
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
            File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(userSettings));
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            SaveUserSettings();
        }

        protected override void OnResume()
        {
        }
    }
}

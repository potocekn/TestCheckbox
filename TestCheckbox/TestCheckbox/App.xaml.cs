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
        UserSettings userSettings;

        Dictionary<string, string> shortcuts = new Dictionary<string, string>();
        public App()
        {
            shortcuts.Add("English", "en");
            shortcuts.Add("German", "de");
            shortcuts.Add("Czech", "cs");
            shortcuts.Add("French", "fr");
            shortcuts.Add("Chinese", "zh-Hans");
            InitializeComponent();

            userSettings = RetrieveUserSettings(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            SetAppLanguage(userSettings.AppLanguage);
            
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
            MainPage = new NavigationPage(new MainPage(this, languageName));  
            
        }

        private UserSettings RetrieveUserSettings(string path)
        {
            UserSettings result = new UserSettings(path);
            if (File.Exists(fileName))
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText(fileName).Trim());
            }          
            
            return result;
        }

        public void ReloadApp(string language, string previouslyChecked)
        {
            WasRefreshed = true;
            userSettings.AppLanguage = previouslyChecked;
            Application.Current.Properties["currentLanguage"] = language;
            File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(userSettings));
            MainPage = new NavigationPage(new MainPage(this, previouslyChecked));
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

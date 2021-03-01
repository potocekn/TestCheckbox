using System;
using Xamarin.Forms;
using System.Globalization;
using Xamarin.Forms.Xaml;
using System.Threading;
using AppBase.Resources;
using System.IO;
using System.Collections.Generic;

namespace TestCheckbox
{
    public partial class App : Application
    {
        public bool IsFirst = true;
        public bool WasRefreshed = false;
        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "userSettings.txt");

        Dictionary<string, string> shortcuts = new Dictionary<string, string>();
        public App()
        {
            shortcuts.Add("English", "en");
            shortcuts.Add("German", "de");
            shortcuts.Add("Czech", "cs");
            shortcuts.Add("French", "fr");
            shortcuts.Add("Chinese", "zh-Hans");
            InitializeComponent();

            if (File.Exists(fileName))
            {
                string previousLanguage = File.ReadAllText(fileName).Trim();
                string shortcut = "";
                bool success = shortcuts.TryGetValue(previousLanguage, out shortcut);
                CultureInfo language;
                if (success)
                {
                    language = new CultureInfo(shortcut);
                }
                else
                {
                   language  = new CultureInfo("en");
                }               

                Thread.CurrentThread.CurrentUICulture = language;
                AppResources.Culture = language;
                MainPage = new NavigationPage(new MainPage(this, previousLanguage));
            }
            else
            {
                CultureInfo language = new CultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = language;
                AppResources.Culture = language;
                MainPage = new NavigationPage(new MainPage(this, "English"));
            }

            
        }

        public void ReloadApp(string language, string previouslyChecked)
        {
            WasRefreshed = true;
            Application.Current.Properties["currentLanguage"] = language;
            File.WriteAllText(fileName, previouslyChecked);
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

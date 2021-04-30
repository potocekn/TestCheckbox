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
        bool firstTimeRunning = true;
        string userSettingsfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "userSettings.json");
        string resourcesfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "resources.json");

        public bool IsFirst = true;
        public bool WasRefreshed = false;
        public UserSettings userSettings;
        public List<ResourcesInfoPDF> resourcesPDF;
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
            RetrieveResources();
            Thread.Sleep(6000);
            SaveHtmlToDbs();
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

        private void RetrieveResources()
        {
            List<ResourcesInfoPDF> resourcesInfos = new List<ResourcesInfoPDF>();

            if (File.Exists(resourcesfileName))
            {
                resourcesInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResourcesInfoPDF>>(File.ReadAllText(resourcesfileName).Trim());
            }

            resourcesPDF = resourcesInfos;           
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
                case "Once a Month":
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
            
            if (userSettings.DownloadOnlyWithWifi)
            {
                var profiles = Connectivity.ConnectionProfiles;
                if (profiles.Contains(ConnectionProfile.WiFi))
                {
                    DownloadTestFiles();
                }
            }
            else
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    DownloadTestFiles();
                }
            }      
        }

        private async void SaveHtmlToDbs()
        {
            string base64String = Xamarin.Essentials.Preferences.Get("testImage.png", string.Empty);

            string htmlCode = "<!DOCTYPE html>" +
"<html class=\"client-nojs\" lang=\"en\" dir=\"ltr\">" +
"<head>" +
"<meta charset=\"UTF-8\"/>" +
"<title>" + "Baptism - 4training</title>" +

  "</head>" +
  "<body class=\"mediawiki ltr sitedir-ltr mw-hide-empty-elt ns-0 ns-subject page-Baptism rootpage-Baptism skin-fortraining action-view skin-vector-legacy\">" +

              "<h1 id=\"firstHeading\" class=\"firstHeading\" lang=\"en\">" + "Baptism </h1>" +
                                        "<div id=\"bodyContent\" class=\"mw-body-content\">" +
                                        "<div id=\"siteSub\">" + "From 4training</div>" +
                                      "<div id=\"contentSub\">" + "</div>" +
"<img src=\"data:image/png;base64," + "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" + "\" width=\"300\" height=\"200\"></img>" +
"<h2>" + "<span class=\"mw-headline\" id=\"Story\">" + "Story</span>" + "</h2>" +
    "<p>" + "(Matthew 3:11, 13 - 17; 28:18 - 20)" +
"</p>" + "<p>" + "<i>" + "Just before Jesus began to teach and heal people, He went out to the Jordan River to be baptized.A prophet named John was there calling people to turn from their sins because the Savior was coming soon.Jesus was that Savior they had been waiting for!</i>" +
    "</p>" + "<p>" + "<i>" + "Jesus had no sins to repent from, but He wanted to be baptized by John in order to be an example for us to follow and to show that He agreed with John’s message.At first John didn’t want to baptize Jesus and told Him, “I need to be baptized by you!” John knew that Jesus was much greater than him.However, after Jesus told John that it was the right thing to do, John agreed to baptize him.</i>" +
        "</p>" + "<p>" + "<i>" + "John baptized Jesus. So Jesus went down under the water and when He came up out of the water, God’s voice from heaven said, “This is my Son whom I love; with Him I am well pleased.”</i>" +
              "</p>" + "<p>" + "<i>" + "At the end of His ministry on earth, Jesus commanded His followers to go and make disciples of all peoples of the world and to baptize them in the name of the Father, Son, and Holy Spirit.They were also to teach them to obey everything Jesus commanded them. His disciples did as commanded, and everywhere they went, they baptized those who decided to become Jesus’ followers.</i>" +
                   "</p>" + "<p>" + "<b>" + "Practice</b>" + "retelling the story!" +
                          "</p>" +
                          "<h3>" + "<span class=\"mw-headline\" id=\"Questions\">" + "Questions</span>" + "</h3>" +
                              "<ol>" + "<li>" + "What do you learn about baptism from this story ? </li>" +
                                  "<li>" + "What should you obey?</li>" + "</ol>" +
                                    "<h2>" + "<span class=\"mw-headline\" id=\"The_meaning_of_baptism\">" + "The meaning of baptism</span>" + "</h2>" +
                                        "<p>" + "The word “baptism” means “to immerse, submerge” as cleansing or washing. Just as Jesus was baptized, everyone who believes in Him needs to be baptized as well.<br />" +
                                          "Jesus commands His followers at the end of the Gospel of Matthew: <br />" +
"“... baptize them in the name of the Father and of the Son and of the Holy Spirit.” (Matthew 28:19)<br />" +
"The meaning of this verse becomes clear in Acts 2:38(memory verse):" +
"</p>" + "<p>" + "<i>" + "Peter replied, “Each of you must repent of your sins and turn to God, and be baptized in the name of Jesus Christ for the forgiveness of your sins.Then you will receive the gift of the Holy Spirit.”</i>" +
      "</p>" +
      "<p style=\"margin-top:1em;margin-bottom:-1em;text-align:center\">" + "<i>" + "Cleansing in the name of the Father...</i>" + "</p>" +
      "<h3>" + "<span class=\"mw-headline\" id=\"Confessing_sins_and_repentance\">" + "Confessing sins and repentance</span>" + "</h3>" +
      "<p>" + "We confess our sins and turn away from them.We don’t sweep our mistakes under the carpet, but we name and confess them(1 John 1:9).We speak out where we lived against God’s will.We ask God for forgiveness and then stop doing these things.With God’s help we change our thinking and behavior and live according to God’s will." +
    "</p>" +
    "<p style=\"margin-top:1em;margin-bottom:-1em;text-align:center\">" + "<i>" + "Cleansing in the name of the Son...</i>" + "</p>" +
    "<h3>" + "<span class=\"mw-headline\" id=\"Water_baptism_in_the_name_of_Jesus_Christ\">" + "Water baptism in the name of Jesus Christ</span>" + "</h3>" +
    "<p>" + "Water baptism is also called the “washing of rebirth” (Titus 3:5).<br />" +
      "Romans 6:1 - 11 explains this meaning: <br />" +
         "In the same way that Jesus was buried and then rose again to life, we go under water in baptism and come out of the water with a new life.Our old sinful nature dies and we’re no longer “slaves of sin”. That means we no longer have to sin. We’re now a “new creation” (2 Corinthians 5:17). In baptism we bury our old life and our new life begins, a totally new lifestyle guided by the example of Jesus." +
"</p>" +
"<p style=\"margin-top:1em;margin-bottom:-1em;text-align:center\">" + "<i>" + "Cleansing in the name of the Holy Spirit...</i>" + "</p>" +
    "<h3>" + "<span id=\"Receiving_God’s_Spirit\">" + "</span>" + "<span class=\"mw-headline\" id=\"Receiving_God.E2.80.99s_Spirit\">" + "Receiving God’s Spirit</span>" + "</h3>" +
            "<p>" + "God wants to give us His Spirit. The Holy Spirit is like “God’s power” for us: He helps us to do God’s will and to resist the devil.He causes good fruits to grow in us like love, joy, peace and patience(Galatians 5:22).<br />" +
            "When we receive God’s Spirit, something happens in us and this becomes obvious to the outside as well (example: Acts 19:6). We receive supernatural gifts(1 Corinthians 12:1 - 11 and 14:1 - 25). These are a support for us and we use them so that others can experience God’s power as well and we can disciple them." +
               "</p>" +
               "<h2>" + "<span class=\"mw-headline\" id=\"Preparing_for_your_baptism\">" + "Preparing for your baptism</span>" + "</h2>" +
                   "<p>" + "You can celebrate your faith at your baptism!" +
                   "</p>" +
                   "<ul>" + "<li>" + "When should the baptism be ? </li>" +
                   "<li>" + "Whom should we invite ? </li>" +
                   "<li>" + "At your baptism you can prepare your story with God to tell everybody how God rescued and changed you.</li>" + "</ul>" +
                   "<p>" + "Set a time to be baptized as soon as possible.Go through the baptism questions and resolve any questions." +
                   "</p>" +
                   "<h2>" + "<span class=\"mw-headline\" id=\"Baptism_questions\">" + "Baptism questions</span>" + "</h2>" +
                   "<ol>" + "<li>" + "Did you confess your sins to God ? </li>" +
                   "<li>" + "Do you know and believe that God has forgiven all your sins through Jesus’ sacrifice ? </li>" +
                   "<li>" + "Are you ready to bury your old life and start a new life with God ? </li>" +
                   "<li>" + "Are you committed to following Jesus and never turning back ? </li>" +
                   "<li>" + "Will you keep following Jesus even if they mock you, beat you, your family kicks you out, or you’ll have other difficulties?</li>" +
                     "<li>" + "Do you want to receive the Holy Spirit ? </li>" + "</ol>" +
                       "<p>" + "<br />" +
                       "</p>" + "<p>" + "<br />" +
                       "</p>" +
                       "</body>" +
                       "</html>";
            HtmlRecord record1 = new HtmlRecord { 
                PageContent = htmlCode,
                PageName = "Prayer3",
                PageLanguage = "English"
            };

            await Database.SavePageAsync(record1);

            string htmlCode2 = "<!DOCTYPE html>" +
"<html class=\"client-nojs\" lang=\"en\" dir=\"ltr\">" +
"<head>" +
"<meta charset=\"UTF-8\"/>" +
"<title>" + "Baptism - 4training</title>" +

  "</head>" +
  "<body class=\"mediawiki ltr sitedir-ltr mw-hide-empty-elt ns-0 ns-subject page-Baptism rootpage-Baptism skin-fortraining action-view skin-vector-legacy\">" +

              "<h1 id=\"firstHeading\" class=\"firstHeading\" lang=\"en\">" + "Baptism </h1>" +
                                        "<div id=\"bodyContent\" class=\"mw-body-content\">" +
                                        "<div id=\"siteSub\">" + "From 4training</div>" +
                                      "<div id=\"contentSub\">" + "</div>" +
"<h2>" + "<span class=\"mw-headline\" id=\"Story\">" + "Story</span>" + "</h2>" +
    "<p>" + "(Matthew 3:11, 13 - 17; 28:18 - 20)" +
"</p>" + "<p>" + "<i>" + "Just before Jesus began to teach and heal people, He went out to the Jordan River to be baptized.A prophet named John was there calling people to turn from their sins because the Savior was coming soon.Jesus was that Savior they had been waiting for!</i>" +
    "</p>" + "<p>" + "<i>" + "Jesus had no sins to repent from, but He wanted to be baptized by John in order to be an example for us to follow and to show that He agreed with John’s message.At first John didn’t want to baptize Jesus and told Him, “I need to be baptized by you!” John knew that Jesus was much greater than him.However, after Jesus told John that it was the right thing to do, John agreed to baptize him.</i>" +
        "</p>" + "<p>" + "<i>" + "John baptized Jesus. So Jesus went down under the water and when He came up out of the water, God’s voice from heaven said, “This is my Son whom I love; with Him I am well pleased.”</i>" +
              "</p>" + "<p>" + "<i>" + "At the end of His ministry on earth, Jesus commanded His followers to go and make disciples of all peoples of the world and to baptize them in the name of the Father, Son, and Holy Spirit.They were also to teach them to obey everything Jesus commanded them. His disciples did as commanded, and everywhere they went, they baptized those who decided to become Jesus’ followers.</i>" +
                   "</p>" + "<p>" + "<b>" + "Practice</b>" + "retelling the story!" +
                          "</p>" +
                          "<h3>" + "<span class=\"mw-headline\" id=\"Questions\">" + "Questions</span>" + "</h3>" +
                              "<ol>" + "<li>" + "What do you learn about baptism from this story ? </li>" +
                                  "<li>" + "What should you obey?</li>" + "</ol>" +
                                    "<h2>" + "<span class=\"mw-headline\" id=\"The_meaning_of_baptism\">" + "The meaning of baptism</span>" + "</h2>" +
                                        "<p>" + "The word “baptism” means “to immerse, submerge” as cleansing or washing. Just as Jesus was baptized, everyone who believes in Him needs to be baptized as well.<br />" +
                                          "Jesus commands His followers at the end of the Gospel of Matthew: <br />" +
"“... baptize them in the name of the Father and of the Son and of the Holy Spirit.” (Matthew 28:19)<br />" +
"The meaning of this verse becomes clear in Acts 2:38(memory verse):" +
"</p>" + "<p>" + "<i>" + "Peter replied, “Each of you must repent of your sins and turn to God, and be baptized in the name of Jesus Christ for the forgiveness of your sins.Then you will receive the gift of the Holy Spirit.”</i>" +
      "</p>" +
      "<p style=\"margin-top:1em;margin-bottom:-1em;text-align:center\">" + "<i>" + "Cleansing in the name of the Father...</i>" + "</p>" +
      "<h3>" + "<span class=\"mw-headline\" id=\"Confessing_sins_and_repentance\">" + "Confessing sins and repentance</span>" + "</h3>" +
      "<p>" + "We confess our sins and turn away from them.We don’t sweep our mistakes under the carpet, but we name and confess them(1 John 1:9).We speak out where we lived against God’s will.We ask God for forgiveness and then stop doing these things.With God’s help we change our thinking and behavior and live according to God’s will." +
    "</p>" +
    "<p style=\"margin-top:1em;margin-bottom:-1em;text-align:center\">" + "<i>" + "Cleansing in the name of the Son...</i>" + "</p>" +
    "<h3>" + "<span class=\"mw-headline\" id=\"Water_baptism_in_the_name_of_Jesus_Christ\">" + "Water baptism in the name of Jesus Christ</span>" + "</h3>" +
    "<p>" + "Water baptism is also called the “washing of rebirth” (Titus 3:5).<br />" +
      "Romans 6:1 - 11 explains this meaning: <br />" +
         "In the same way that Jesus was buried and then rose again to life, we go under water in baptism and come out of the water with a new life.Our old sinful nature dies and we’re no longer “slaves of sin”. That means we no longer have to sin. We’re now a “new creation” (2 Corinthians 5:17). In baptism we bury our old life and our new life begins, a totally new lifestyle guided by the example of Jesus." +
"</p>" +
"<p style=\"margin-top:1em;margin-bottom:-1em;text-align:center\">" + "<i>" + "Cleansing in the name of the Holy Spirit...</i>" + "</p>" +
    "<h3>" + "<span id=\"Receiving_God’s_Spirit\">" + "</span>" + "<span class=\"mw-headline\" id=\"Receiving_God.E2.80.99s_Spirit\">" + "Receiving God’s Spirit</span>" + "</h3>" +
            "<p>" + "God wants to give us His Spirit. The Holy Spirit is like “God’s power” for us: He helps us to do God’s will and to resist the devil.He causes good fruits to grow in us like love, joy, peace and patience(Galatians 5:22).<br />" +
            "When we receive God’s Spirit, something happens in us and this becomes obvious to the outside as well (example: Acts 19:6). We receive supernatural gifts(1 Corinthians 12:1 - 11 and 14:1 - 25). These are a support for us and we use them so that others can experience God’s power as well and we can disciple them." +
               "</p>" +
               "<h2>" + "<span class=\"mw-headline\" id=\"Preparing_for_your_baptism\">" + "Preparing for your baptism</span>" + "</h2>" +
                   "<p>" + "You can celebrate your faith at your baptism!" +
                   "</p>" +
                   "<ul>" + "<li>" + "When should the baptism be ? </li>" +
                   "<li>" + "Whom should we invite ? </li>" +
                   "<li>" + "At your baptism you can prepare your story with God to tell everybody how God rescued and changed you.</li>" + "</ul>" +
                   "<p>" + "Set a time to be baptized as soon as possible.Go through the baptism questions and resolve any questions." +
                   "</p>" +
                   "<h2>" + "<span class=\"mw-headline\" id=\"Baptism_questions\">" + "Baptism questions</span>" + "</h2>" +
                   "<ol>" + "<li>" + "Did you confess your sins to God ? </li>" +
                   "<li>" + "Do you know and believe that God has forgiven all your sins through Jesus’ sacrifice ? </li>" +
                   "<li>" + "Are you ready to bury your old life and start a new life with God ? </li>" +
                   "<li>" + "Are you committed to following Jesus and never turning back ? </li>" +
                   "<li>" + "Will you keep following Jesus even if they mock you, beat you, your family kicks you out, or you’ll have other difficulties?</li>" +
                     "<li>" + "Do you want to receive the Holy Spirit ? </li>" + "</ol>" +
                       "<p>" + "<br />" +
                       "</p>" + "<p>" + "<br />" +
                       "</p>" +
                       "</body>" +
                       "</html>";
            HtmlRecord record2 = new HtmlRecord
            {
                PageContent = htmlCode2,
                PageName = "Prayer2",
                PageLanguage = "English"
            };

            await Database.SavePageAsync(record2);
        }

        private void DownloadPDFFiles()
        {
            IDownloader downloader = DependencyService.Get<IDownloader>();
            foreach (var item in resourcesPDF)
            {
                string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), item.Language);
                downloader.DownloadFile(item.Url, dir, item.FileName);
            }  
        }

        private async void DownloadHTMLFiles(string url)
        {
            foreach (var item in resourcesHTML)
            {
                string contents;
                using (var wc = new System.Net.WebClient())
                {
                    //key == language, value == name
                    contents = wc.DownloadString(url + "/" + item.Key + "/" + item.Value);
                }

                HtmlRecord record = new HtmlRecord
                {
                    PageContent = contents,
                    PageName = item.Value,
                    PageLanguage = item.Key
                };

                await Database.SavePageAsync(record);

            }
        }

        private async void DownloadTestFiles()
        {           
            resourcesPDF = new List<ResourcesInfoPDF>();

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
            resourcesPDF.Add(item);

            fileName = Path.Combine(dir, "test2.pdf");
            ResourcesInfoPDF item2 = new ResourcesInfoPDF()
            {
                Language = "English",
                ResourceName = "Test Resource 2",
                FileName = "test2.pdf",
                Url = "http://www.4training.net/mediawiki/images/8/8b/Baptism.pdf",
                FilePath = fileName
            };
            resourcesPDF.Add(item2);

            fileName = Path.Combine(dir, "test3.odt");
            ResourcesInfoPDF item3 = new ResourcesInfoPDF()
            {
                Language = "English",
                ResourceName = "Test Resource ODT",
                FileName = "test3.odt",
                Url = "https://www.4training.net/mediawiki/images/a/a8/Church.odt",
                FilePath = fileName
            };
            resourcesPDF.Add(item3);

            var downloadedImage = await ImageService.DownloadImage("https://www.4training.net/mediawiki/images/3/3b/Relationship_Triangle.png");

            ImageService.SaveToDisk("testImage.png", downloadedImage);            
        }

        public void SaveUserSettings()
        {
            File.WriteAllText(userSettingsfileName, Newtonsoft.Json.JsonConvert.SerializeObject(userSettings));
        }

        public void SaveResources()
        {
            File.WriteAllText(resourcesfileName, Newtonsoft.Json.JsonConvert.SerializeObject(resourcesPDF));
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

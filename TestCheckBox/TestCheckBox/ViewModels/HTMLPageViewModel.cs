using AppBase.Interfaces;
using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    class HTMLPageViewModel
    {
        App app { get; set; }

        public HtmlWebViewSource HTMLSource { get; set; }
        public Command OpenResource { get; set; }

        public HTMLPageViewModel(App app)
        {
            this.app = app;
            this.HTMLSource = new HtmlWebViewSource();
            GetSource("http://www.4training.net/Special:MyLanguage/Baptism");
        }

        void GetSource(string url)
        {
            //IDownloader downloader = DependencyService.Get<IDownloader>();
            //string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Images");

            //downloader.DownloadFile("https://www.4training.net/mediawiki/images/3/3b/Relationship_Triangle.png", dir, "testImage.png");
            //string pathToFile = Path.Combine(dir, "testImage.png");
            ////string img = "http://www.4training.net/File:Relationship_Triangle.png";
            //string img = "file://" + pathToFile;
            //byte[] image = new byte[0];
            //try
            //{
            //    image = File.ReadAllBytes(pathToFile);
            //}
            //catch (Exception e)
            //{

            //}


            /*using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                //client.DownloadFile("http://yoursite.com/page.html", @"C:\localfile.html");

                // Or you can get the file content without saving it
                htmlCode = client.DownloadString(url);
            }*/
            var record = App.Database.GetPageAsync("Prayer3");
            HTMLSource.Html = record.Result.PageContent;
            //HTMLSource.BaseUrl = "https://www.4training.net/mediawiki/images/3/3b/Relationship_Triangle.png";
        }
    }
}

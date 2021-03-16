using AppBaseNamespace;
using Plugin.XamarinFormsSaveOpenPDFPackage;
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
            string htmlCode = "";
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                //client.DownloadFile("http://yoursite.com/page.html", @"C:\localfile.html");

                // Or you can get the file content without saving it
                htmlCode = client.DownloadString(url);
            }
            HTMLSource.Html = htmlCode;
        }
    }
}

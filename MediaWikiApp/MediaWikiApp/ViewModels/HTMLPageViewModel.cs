using AppBase.Interfaces;
using AppBase.Models;
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
    /// <summary>
    /// Class representing HTML Page view model. Class contains its HTML source and a command to open the resource.
    /// </summary>
    class HTMLPageViewModel
    {  
        public HtmlWebViewSource HTMLSource { get; set; }
        public Command OpenResource { get; set; }

        public HTMLPageViewModel(HtmlRecord record)
        {            
            this.HTMLSource = new HtmlWebViewSource();
            HTMLSource.Html = record.PageContent;
        }

        void GetSource(string url)
        {            
            var record = App.Database.GetPageAsync("Prayer3");
            HTMLSource.Html = record.Result.PageContent;            
        }
    }
}

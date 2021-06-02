using AppBase.UserSettingsHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.Models
{
    public class ResourceLanguageInfo
    {
        public string LanguageName { get; set; }
        public List<HtmlRecord> HTMLs { get; set; }
        public List<ResourcesInfoPDF> PDFs { get; set; }
        public List<ResourcesInfoPDF> ODTs { get; set; }
        public Command OpenResources { get; set; }
    }
}

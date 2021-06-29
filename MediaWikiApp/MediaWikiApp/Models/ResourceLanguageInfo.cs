using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.Models
{
    /// <summary>
    /// This class is used for filtering resources only for one language. 
    /// The list of HTML, PDF and ODt files is filtered to contain only those which have the desired language.
    /// </summary>
    public class ResourceLanguageInfo
    {
        public string LanguageName { get; set; }
        public List<HtmlRecord> HTMLs { get; set; }
        public List<ResourcesInfo> PDFs { get; set; }
        public List<ResourcesInfo> ODTs { get; set; }
        public Command OpenResources { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.Models
{
    /// <summary>
    /// Class representing an ODT file that is used by the ODT page. Each instance of this class
    /// has its own button that is used for sharing the resource.
    /// </summary>
    class ODTPageItem
    {
        public string Language { get; set; }
        public string ResourceName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Url { get; set; }      
        public Command ShareResource { get; set; }
    }
}

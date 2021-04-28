using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// Class representing one pdf resource item used in pdf page view model. 
    /// Item consists of the language of the resource, its name, name of the file where the resource is stored, path to the file
    /// and commands to open and share file.
    /// </summary>
    class PDFPageItem
    {
        public string Language { get; set; }
        public string ResourceName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Url { get; set; }
        public Command OpenResource { get; set; }
        public Command ShareResource { get; set; }
    }
}

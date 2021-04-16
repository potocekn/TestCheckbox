using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    class PDFPageItemViewModel
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

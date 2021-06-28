using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AppBase;
using Xamarin.Forms;

namespace AppBase
{
    /// <summary>
    /// Class that uses pdfjs library to display PDF files.
    /// </summary>
    public partial class pdfjsPage: ContentPage
    {
        public pdfjsPage(string path)
        {
            InitializeComponent();

            var localPath = string.Empty;

            if (Device.RuntimePlatform == Device.Android)
            {                
                localPath = path;                
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                PdfView.Source = $"file:///android_asset/pdfjs/web/viewer.html?file={WebUtility.UrlEncode(localPath)}";
            }
            else
            {
                PdfView.Source = path;
            }                
        }
    }
}
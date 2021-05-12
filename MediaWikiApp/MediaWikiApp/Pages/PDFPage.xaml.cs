using AppBase.Interfaces;
using AppBase.UserSettingsHelpers;
using AppBase.ViewModels;
using AppBaseNamespace;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PDFPage : ContentPage
    {
        IDownloader downloader = DependencyService.Get<IDownloader>();
        public PDFPage(List<ResourcesInfoPDF> resources)
        {
            InitializeComponent();
            //downloader.OnFileDownloaded += OnFileDownloaded;
            //DownloadFiles(downloader, resources);
            BindingContext = new PDFPageViewModel(Navigation, resources);
        }

        private void DownloadFiles(IDownloader downloader, List<ResourcesInfoPDF> resources)
        {
            foreach (var item in resources)
            {
                string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), item.Language);
                downloader.DownloadFile(item.Url, dir, item.FileName);
            }                       
        }

        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                DisplayAlert("XF Downloader", "File Saved Successfully", "Close");
            }
            else
            {
                DisplayAlert("XF Downloader", "Error while saving the file", "Close");
            }
        }      
    }
}
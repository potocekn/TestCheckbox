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
        public PDFPage(List<ResourcesInfo> resources)
        {
            InitializeComponent();
            downloader.OnFileDownloaded += OnFileDownloaded;
            DownloadFiles(downloader, resources);
            BindingContext = new PDFPageViewModel(Navigation, resources);
        }

        private void DownloadFiles(IDownloader downloader, List<ResourcesInfo> resources)
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

        private void SaveFile(PDFPageItem resource)
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), resource.Language);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string fileName = Path.Combine(dir, resource.FileName);
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
                resource.FilePath = fileName;
                using (var client = new WebClient())
                {
                    try
                    {
                        //client.DownloadFile(resource.Path, fileName);
                        client.DownloadStringCompleted += (s, e) => {
                            var text = e.Result; // get the downloaded text                            
                            File.WriteAllText(fileName, text); // writes to local storage
                            Console.WriteLine(text);
                        };
                        var url = new Uri(resource.Url); // Html home page
                        client.Encoding = Encoding.UTF8;
                        client.DownloadStringAsync(url);
                    }
                    catch (Exception e)
                    {

                    }

                }
            }
        }
               
    }
}
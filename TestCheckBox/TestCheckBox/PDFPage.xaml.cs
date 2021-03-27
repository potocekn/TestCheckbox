using AppBase.Interfaces;
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
        public PDFPage(App app)
        {
            InitializeComponent();
            downloader.OnFileDownloaded += OnFileDownloaded;
            DownloadFiles(downloader);
            BindingContext = new PDFPageViewModel(app, Navigation);
        }

        private void DownloadFiles(IDownloader downloader)
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "English");
            string fileName = Path.Combine(dir, "test.pdf");
            PDFPageItemViewModel item = new PDFPageItemViewModel()
            {
                Language = "English",
                ResourceName = "Test Resource",
                FileName = "test.pdf",
                Path = "http://www.4training.net/mediawiki/images/a/af/Gods_Story_%28five_fingers%29.pdf",
                FilePath = fileName
            };

            downloader.DownloadFile(item.Path, dir, item.FileName);

            fileName = Path.Combine(dir, "test2.pdf");
            PDFPageItemViewModel item2 = new PDFPageItemViewModel()
            {
                Language = "English",
                ResourceName = "Test Resource 2",
                FileName = "test2.pdf",
                Path = "http://www.4training.net/mediawiki/images/8/8b/Baptism.pdf",
                FilePath = fileName
            };
            downloader.DownloadFile(item2.Path, dir, item2.FileName);
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

        private void SaveFile(PDFPageItemViewModel resource)
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
                        var url = new Uri(resource.Path); // Html home page
                        client.Encoding = Encoding.UTF8;
                        client.DownloadStringAsync(url);
                    }
                    catch (Exception e)
                    {

                    }

                }
            }
        }

        //public async void OnButtonClickShare(object sender, EventArgs args)
        //{
        //    await (BindingContext as PDFPageViewModel).OnButtonClickShareAsync(sender as Button);
        //}
    }
}
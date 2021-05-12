using AppBase.Interfaces;
using AppBase.UserSettingsHelpers;
using AppBase.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ODTPage : ContentPage
    {
        IDownloader downloader = DependencyService.Get<IDownloader>();
        public ODTPage(List<ResourcesInfoPDF> resources)
        {
            InitializeComponent();           
            DownloadFiles(downloader, resources);
            BindingContext = new ODTPageViewModel(resources);
        }
        private void DownloadFiles(IDownloader downloader, List<ResourcesInfoPDF> resources)
        {
            foreach (var item in resources)
            {
                string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), item.Language);
                downloader.DownloadFile(item.Url, dir, item.FileName);
            }
        }
    }
}
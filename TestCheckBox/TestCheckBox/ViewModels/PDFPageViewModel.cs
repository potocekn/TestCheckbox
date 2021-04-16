using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using PCLStorage;
using System.Diagnostics;
using AppBase.UserSettingsHelpers;

namespace AppBase.ViewModels
{
    class PDFPageViewModel
    {
        App app { get; set; }

        public List<PDFPageItemViewModel> Items { get; set; }

        public PDFPageViewModel(App app, INavigation navigation, List<ResourcesInfo> resources)
        {
            this.app = app;
            Items = new List<PDFPageItemViewModel>();

            foreach (var resource in resources)
            {
                PDFPageItemViewModel itemViewModel = new PDFPageItemViewModel()
                {
                    Language = resource.Language,
                    ResourceName = resource.ResourceName,
                    FileName = resource.FileName,
                    Url = resource.Url,
                    FilePath = resource.FilePath,
                    OpenResource = new Command(() => { navigation.PushAsync(new pdfjsPage(resource.FilePath)); }),
                    ShareResource = new Command(async () => { await OnButtonClickShareAsync(resource.FilePath); })
                };
                Items.Add(itemViewModel);
            }
        }


        public async Task OnButtonClickShareAsync(string filePath)
        {           
            
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share",
                File = new ShareFile(filePath)
            });
            
        }

    }    
}

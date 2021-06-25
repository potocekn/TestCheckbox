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
using System.Diagnostics;
using AppBase.UserSettingsHelpers;

namespace AppBase.ViewModels
{
    /// <summary>
    /// Class representing view model of the PDF page. The model remembers all PDF resources. 
    /// </summary>
    class PDFPageViewModel
    {     
        public List<PDFPageItem> Items { get; set; }

        public PDFPageViewModel(INavigation navigation, List<ResourcesInfoPDF> resources)
        {           
            Items = new List<PDFPageItem>();

            foreach (var resource in resources)
            {
                PDFPageItem itemViewModel = new PDFPageItem()
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

        /// <summary>
        /// Method used for sharing the resource specified with path.
        /// </summary>
        /// <param name="filePath">path to the file that will be shared</param>
        /// <returns></returns>
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

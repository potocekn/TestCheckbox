using AppBase.Models;
using AppBase.UserSettingsHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// Class that is used as view model for the ODT Page. 
    /// This view model is responsible for providing data for the displaying of the available oDT resources
    /// and to call share dialog when clicked on the buttons representing the resources.
    /// </summary>
    class ODTPageViewModel
    {
        public List<ODTPageItem> Items { get; set; }
        public ODTPageViewModel(List<ResourcesInfoPDF> resources)
        {
            Items = new List<ODTPageItem>();

            foreach (var resource in resources)
            {
                ODTPageItem itemViewModel = new ODTPageItem()
                {
                    Language = resource.Language,
                    ResourceName = resource.ResourceName,
                    FileName = resource.FileName,
                    Url = resource.Url,
                    FilePath = resource.FilePath,                    
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

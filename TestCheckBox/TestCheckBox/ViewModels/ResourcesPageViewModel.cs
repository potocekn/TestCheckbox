using AppBaseNamespace;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    public class ResourcesPageViewModel
    {
        App app { get; set; }
        public Command GoToPDFs { get; set; }
        public Command GoToHTMLs { get; set; }
        public Command GoToHTMLsLabel { get; set; }

        public ResourcesPageViewModel(App app, INavigation navigation)
        {
            this.app = app;
            GoToPDFs = new Command(() => {
                navigation.PushAsync(new PDFPage(app));
            });
            GoToHTMLs = new Command(() => {
                navigation.PushAsync(new HTMLPage(app));
            });
        }

        async void LoadAndOpenResource()
        {
            var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync("http://www.4training.net/mediawiki/images/a/af/Gods_Story_%28five_fingers%29.pdf");

            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView("test.pdf", "application/pdf", memoryStream, PDFOpenContext.InApp);
            }

                
        }
    }
}

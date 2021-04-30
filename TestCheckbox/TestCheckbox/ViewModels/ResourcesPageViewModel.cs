using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// Class representing the vie model of the resources page. The model contains commands that go to the PDF or HTML resources.
    /// </summary>
    public class ResourcesPageViewModel
    {
        App app { get; set; }
        public Command GoToPDFs { get; set; }
        public Command GoToHTMLs { get; set; }        

        public ResourcesPageViewModel(App app, INavigation navigation)
        {
            this.app = app;
            GoToPDFs = new Command(() => {
                navigation.PushAsync(new PDFPage(app.resourcesPDF));
            });
            GoToHTMLs = new Command(() => {
                navigation.PushAsync(new HTMLResourcesPage());
            });            
        }                
    }
}

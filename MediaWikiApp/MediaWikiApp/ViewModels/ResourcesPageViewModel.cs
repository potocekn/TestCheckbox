using AppBase.Helpers;
using AppBase.Models;
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
    /// Class representing the view model of the resources page. 
    /// The model contains commands that go to the PDF or HTML resources.
    /// </summary>
    public class ResourcesPageViewModel
    {       
        public bool IsPDFSelected { get; set; }
        public bool IsHTMLSelected { get; set; }
        public bool IsODTSelected { get; set; }
        public Command GoToPDFs { get; set; }
        public Command GoToHTMLs { get; set; }
        public Command GoToODTs { get; set; }

        public ResourcesPageViewModel(App app, INavigation navigation)
        {
            IsHTMLSelected = app.userSettings.Formats.Contains(Constants.HTML);
            IsPDFSelected = app.userSettings.Formats.Contains(Constants.PDF);
            IsODTSelected = app.userSettings.Formats.Contains(Constants.ODT);
            GoToPDFs = new Command(() => {
                navigation.PushAsync(new PDFPage(app.resourcesPDF));
            });
            GoToHTMLs = new Command(() => {
                navigation.PushAsync(new HTMLResourcesPage());
            });
            GoToODTs = new Command(() => {
                navigation.PushAsync(new ODTPage(app.resourcesODT));
            });
        }

        public ResourcesPageViewModel(ResourceLanguageInfo resourceLanguageInfo, INavigation navigation)
        {
            IsHTMLSelected = !(resourceLanguageInfo.HTMLs.Count == 0);
            IsPDFSelected = !(resourceLanguageInfo.PDFs.Count == 0);
            IsODTSelected = !(resourceLanguageInfo.ODTs.Count == 0);
            GoToPDFs = new Command(() => {
                navigation.PushAsync(new PDFPage(resourceLanguageInfo.PDFs));
            });
            GoToHTMLs = new Command(() => {
                navigation.PushAsync(new HTMLResourcesPage(resourceLanguageInfo.HTMLs));
            });
            GoToODTs = new Command(() => {
                navigation.PushAsync(new ODTPage(resourceLanguageInfo.ODTs));
            });
        }
    }
}

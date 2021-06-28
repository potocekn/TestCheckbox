using AppBase.Interfaces;
using AppBase.UserSettingsHelpers;
using AppBase.ViewModels;
using AppBaseNamespace;
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
    /// <summary>
    /// Class representing page that displays list of all available PDF files.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PDFPage : ContentPage
    {
        public PDFPage(List<ResourcesInfoPDF> resources)
        {
            InitializeComponent();            
            BindingContext = new PDFPageViewModel(Navigation, resources);
        }      
    }
}
using AppBase.Interfaces;
using AppBase.ViewModels;
using AppBase.Models;
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
    /// <summary>
    /// Class used for diplaying the list of all available ODT resources.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ODTPage : ContentPage
    {
        public ODTPage(List<ResourcesInfo> resources)
        {
            InitializeComponent();           
            BindingContext = new ODTPageViewModel(resources);
        }        
    }
}
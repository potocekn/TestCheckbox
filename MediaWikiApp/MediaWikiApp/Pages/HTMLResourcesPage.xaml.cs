using AppBase.Models;
using AppBase.ViewModels;
using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBase
{
    /// <summary>
    /// Class used for displaying of the available HTML resources list.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HTMLResourcesPage : ContentPage
    {
        public HTMLResourcesPage()
        {
            InitializeComponent();
            var records = App.Database.GetPagesAsync();
            BindingContext = new HTMLResourcesPageViewModel(Navigation, records.Result);
        }

        public HTMLResourcesPage(List<HtmlRecord> records)
        {
            InitializeComponent();            
            BindingContext = new HTMLResourcesPageViewModel(Navigation, records);
        }
    }
}
using AppBase.Interfaces;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HTMLPage : ContentPage
    {
        public HTMLPage(HtmlRecord record)
        {
            InitializeComponent();            
            BindingContext = new HTMLPageViewModel(record);            
            webView.Source = (BindingContext as HTMLPageViewModel).HTMLSource;
        }
    }
}
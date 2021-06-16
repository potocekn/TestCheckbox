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
    public partial class FirstRunDownloadResourcesPage : ContentPage
    {
        App app;
        public FirstRunDownloadResourcesPage(App app)
        {
            this.app = app;
            InitializeComponent();
            BindingContext = new FirstRunDownloadResourcesPageViewModel();
        }
        public async void OnButtonClicked(object sender, EventArgs args)
        {
            (sender as Button).IsEnabled = false;
            (sender as Button).TextColor = Color.Gray;           
            bool downloaded = await (BindingContext as FirstRunDownloadResourcesPageViewModel).Download(app, this);
            if (!downloaded)
            {
                (sender as Button).IsEnabled = true;
                (sender as Button).TextColor = Color.White;
            }
        }
    }
}
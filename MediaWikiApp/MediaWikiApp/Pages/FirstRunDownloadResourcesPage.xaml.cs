using AppBase.Helpers;
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
    /// Class used for the download phase of the configuration process.
    /// During this phase the resources are downloaded based on the configuration.
    /// </summary>
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

        /// <summary>
        /// Method used when clicked on the button.
        /// </summary>
        /// <param name="sender">Button that was clicked on</param>
        /// <param name="args">Event arguments</param>
        public async void OnButtonClicked(object sender, EventArgs args)
        {
            (sender as Button).IsEnabled = false;
            (sender as Button).TextColor = Color.Gray;         
            bool downloaded = await (BindingContext as FirstRunDownloadResourcesPageViewModel).Download(app, this);
            if (!downloaded)
            {
                (sender as Button).IsEnabled = true;
                (sender as Button).TextColor = Color.FromHex(Constants.BUTTON_ENABLED_TEXT_COLOR);
            }
        }
    }
}
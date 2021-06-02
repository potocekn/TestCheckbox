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
    public partial class ResourcesPage : ContentPage
    {
        public ResourcesPage(App app, INavigation navigation)
        {
            InitializeComponent();
            BindingContext = new ResourcesPageViewModel(app, navigation);
        }

        public ResourcesPage(ResourceLanguageInfo resourceLanguageInfo, INavigation navigation)
        {
            InitializeComponent();
            BindingContext = new ResourcesPageViewModel(resourceLanguageInfo, navigation);
        }
    }
}
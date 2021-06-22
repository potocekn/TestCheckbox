using AppBase.ViewModels;
using AppBaseNamespace;
using AppBaseNamespace.ViewModels;
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
    public partial class FileLocationSettingsPage : ContentPage
    {
        public FileLocationSettingsPage(App app)
        {
            InitializeComponent();
            BindingContext = new FileLocationSettingsPageViewModel(app);
        }

        public void OnClicked(object sender, EventArgs e)
        {
        }
    }
}
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
    public partial class ResourceLanguagesFirstRunPage : ContentPage
    {
        public ResourceLanguagesFirstRunPage(App app, List<string> languages)
        {
            InitializeComponent();
            //List<string> languages = new List<string> { "English", "Czech", "German" };
            BindingContext = new ResourceLanguagesFirstRunPageViewModel(app, Navigation, languages);
        }

        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            (BindingContext as ResourceLanguagesFirstRunPageViewModel).OnCheckBoxCheckedChanged(sender, e);
        }
    }
}
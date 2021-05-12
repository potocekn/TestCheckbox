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
    public partial class AppLanguageFirstRunPage : ContentPage
    {

        public AppLanguageFirstRunPage(App app)
        {
            InitializeComponent();
            List<string> englishVersions = new List<string>() { "English", "German", "Czech", "French", "Chinese" };
            List<string> items = new List<string>();
            List<string> shortcuts = new List<string>();
            items.Add(englishLabel.Text);
            shortcuts.Add("en");
            items.Add(germanLabel.Text);
            shortcuts.Add("de");
            items.Add(czechLabel.Text);
            shortcuts.Add("cs");
            items.Add(frenchLabel.Text);
            shortcuts.Add("fr");
            items.Add(chineseLabel.Text);
            shortcuts.Add("zh-Hans");

            BindingContext = new AppLanguageFirstRunPageViewModel(app, Navigation, items, shortcuts, englishVersions);
        }
        
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ((AppLanguageFirstRunPageViewModel)BindingContext).OnCheckBoxCheckedChanged(((sender as CheckBox).BindingContext as LanguageSettingsItem));

        }

        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ((AppLanguageFirstRunPageViewModel)BindingContext).TapGestureRecognizer_Tapped(sender, e);
        }
    }

}
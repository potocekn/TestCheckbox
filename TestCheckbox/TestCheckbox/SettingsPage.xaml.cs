using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCheckbox.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestCheckbox
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        List<string> englishVersions = new List<string>() { "English", "German", "Czech", "French", "Chinese"};
        public SettingsPage( App app, MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
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

            BindingContext = new SettingsPageViewModel(items, shortcuts, englishVersions, this, mainPageViewModel, app);
        }

        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ((SettingsPageViewModel)BindingContext).OnCheckBoxCheckedChangedAsync(((sender as CheckBox).BindingContext as SettingsItemViewModel));

        }
        

        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ((SettingsPageViewModel)BindingContext).TapGestureRecognizer_Tapped(sender, e);
        }      
    }
}
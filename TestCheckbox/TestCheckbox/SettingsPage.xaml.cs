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
        public SettingsPage(IEnumerable<string> items)
        {
            InitializeComponent();
            BindingContext = new SettingsPageViewModel(items, this);
        }
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            //this.DisplayAlert("", "Checked changed", "OK");
            ((SettingsPageViewModel)BindingContext).OnCheckChanged((sender as CheckBox).BindingContext as SettingsItemViewModel);
        }
    }
}
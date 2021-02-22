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
            ((SettingsPageViewModel)BindingContext).OnCheckChanged((sender as CheckBox).BindingContext as SettingsItemViewModel);
        }

        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ((SettingsPageViewModel)BindingContext).TapGestureRecognizer_Tapped(sender, e);
        }      
    }
}
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
    public partial class UpdateIntervalSettingsPage : ContentPage
    {
        public UpdateIntervalSettingsPage(App app, MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
           
            List<UpdateIntervalSettingsItemViewModel> switchNames = new List<UpdateIntervalSettingsItemViewModel>();
            switchNames.Add(new UpdateIntervalSettingsItemViewModel()
            {
                Name = AutomaticOptionLabel.Text,
                WasUpdated = false,
                IsChecked = true
            }) ;
            switchNames.Add(new UpdateIntervalSettingsItemViewModel()
            {
                Name = OnceAMonthOptionLabel.Text,
                WasUpdated = false,
                IsChecked = true
            });
            switchNames.Add(new UpdateIntervalSettingsItemViewModel()
            {
                Name = OnRequestOptionLabel.Text,
                WasUpdated = false,
                IsChecked = true
            });
            BindingContext = new UpdateIntervalSettingsPageViewModel(app, mainPageViewModel, switchNames);
        }

        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            (BindingContext as UpdateIntervalSettingsPageViewModel).OnCheckedChanged(((sender as CheckBox).BindingContext as UpdateIntervalSettingsItemViewModel));
        }
    }
}
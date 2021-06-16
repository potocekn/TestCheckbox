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
        List<UpdateIntervalSettingsItem> Checkboxes = new List<UpdateIntervalSettingsItem>();
        public UpdateIntervalSettingsPage(App app, MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();

            if (app.userSettings.UpdateInterval == englishAutomatic.Text)
            {
                AddItem(AutomaticOptionLabel.Text, true, false, englishAutomatic.Text);
            }
            else
            {
                AddItem(AutomaticOptionLabel.Text, false, false, englishAutomatic.Text);
            }

            if (app.userSettings.UpdateInterval == englishOnceAMonth.Text)
            {
                AddItem(OnceAMonthOptionLabel.Text, true, false, englishOnceAMonth.Text);
            }
            else
            {
                AddItem(OnceAMonthOptionLabel.Text, false, false, englishOnceAMonth.Text);
            }

            if (app.userSettings.UpdateInterval == englishOnRequest.Text)
            {
                AddItem(OnRequestOptionLabel.Text, true, false, englishOnRequest.Text);
            }
            else
            {
                AddItem(OnRequestOptionLabel.Text, false, false, englishOnRequest.Text);
            }           
           
            BindingContext = new UpdateIntervalSettingsPageViewModel(app, Checkboxes);
        }

        void AddItem(string name, bool isChecked, bool wasUpdated, string englishName)
        {
            Checkboxes.Add(new UpdateIntervalSettingsItem()
            {
                Name = name,
                WasUpdated = wasUpdated,
                IsChecked = isChecked, 
                EnglishName = englishName
            });
        }
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            (BindingContext as UpdateIntervalSettingsPageViewModel).OnCheckedChanged(((sender as CheckBox).BindingContext as UpdateIntervalSettingsItem));
        }
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            (BindingContext as UpdateIntervalSettingsPageViewModel).TapGestureRecognizer_Tapped(sender, e);
        }
    }
}
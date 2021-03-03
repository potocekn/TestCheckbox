using AppBaseNamespace;
using AppBaseNamespace.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    public class UpdateIntervalSettingsPageViewModel
    {
        public List<UpdateIntervalSettingsItemViewModel> Items { get; set; }
        MainPageViewModel MainPageViewModelBackup { get; set; }
        App app { get; set; }
        public UpdateIntervalSettingsPageViewModel(App app, MainPageViewModel mainPageViewModel, List<UpdateIntervalSettingsItemViewModel> switches)
        {
            this.app = app;
            MainPageViewModelBackup = mainPageViewModel;
            Items = switches;             
        }
        private void HandleCheckChange(UpdateIntervalSettingsItemViewModel sender)
        {
            sender.IsChecked = true;
            sender.WasUpdated = true;
            app.userSettings.UpdateInterval = sender.EnglishName;
            app.SaveUserSettings();
            foreach (var item in Items)
            {
                if (!item.Equals(sender))
                {
                    item.IsChecked = false;
                    item.WasUpdated = false;
                    item.NotifyPropertyChanged("IsChecked");
                }
            }
        }

        private void CheckAndHandleAllFalse()
        {
            bool allFalse = true;

            foreach (var item in Items)
            {
                if (item.IsChecked)
                {
                    allFalse = false;
                    break;
                }
            }

            if (allFalse)
            {
                UpdateIntervalSettingsItemViewModel automatic = Items.Find(x => (x.Name == "Automatic"));
                automatic.IsChecked = true;
                automatic.WasUpdated = true;
                automatic.NotifyPropertyChanged("IsChecked");
            }
        }
        public void OnCheckedChanged(UpdateIntervalSettingsItemViewModel sender)
        {
            if (sender.WasUpdated == true)
            {
                CheckAndHandleAllFalse();
                return;
            }

            HandleCheckChange(sender);
        }

        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Label label = (sender as Label);
            foreach (var item in Items)
            {
                if (label.Text == item.Name)
                {
                    item.IsChecked = true;
                    break;
                }
            }
        }
    }
}

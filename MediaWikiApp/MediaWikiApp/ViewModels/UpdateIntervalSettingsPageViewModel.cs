using AppBaseNamespace;
using AppBaseNamespace.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// Class representing the view model of the update interval settings page. 
    /// The model remembers all the interval options and languages of the resources.
    /// </summary>
    public class UpdateIntervalSettingsPageViewModel
    {
        public List<UpdateIntervalSettingsItem> Items { get; set; }
        MainPageViewModel MainPageViewModelBackup { get; set; }
        App app { get; set; }
        public UpdateIntervalSettingsPageViewModel(App app, MainPageViewModel mainPageViewModel, List<UpdateIntervalSettingsItem> switches)
        {
            this.app = app;
            MainPageViewModelBackup = mainPageViewModel;
            Items = switches;             
        }

        /// <summary>
        /// Method that handles IsChecked property when the language of resources changes.
        /// </summary>
        /// <param name="sender">object that changed status</param>
        private void HandleCheckChange(UpdateIntervalSettingsItem sender)
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

        /// <summary>
        /// Method that handles situation when all the checkboxes are unchecked.
        /// In that case "Automatic" option is checked.
        /// </summary>
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
                UpdateIntervalSettingsItem automatic = Items.Find(x => (x.Name == "Automatic"));
                automatic.IsChecked = true;
                automatic.WasUpdated = true;
                automatic.NotifyPropertyChanged("IsChecked");
            }
        }

        /// <summary>
        /// Method that handles the change of checkbox status.
        /// </summary>
        /// <param name="sender">object that changed status</param>
        public void OnCheckedChanged(UpdateIntervalSettingsItem sender)
        {
            if (sender.WasUpdated == true)
            {
                CheckAndHandleAllFalse();
                return;
            }

            HandleCheckChange(sender);
        }

        /// <summary>
        /// Method that enables checking checkbox by clicking on the label next to it.
        /// </summary>
        /// <param name="sender">label that was clicked on</param>
        /// <param name="e">event args</param>
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

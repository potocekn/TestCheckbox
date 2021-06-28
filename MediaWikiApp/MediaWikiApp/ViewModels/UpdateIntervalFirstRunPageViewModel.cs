using AppBase.Models;
using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// A class that represents view model for the update interval settings page during the very first run of the application.
    /// </summary>
    public class UpdateIntervalFirstRunPageViewModel
    {
        public List<UpdateIntervalSettingsItem> Items { get; set; }
        public Command GoToDownload { get; set; }
        App app { get; set; }
        public UpdateIntervalFirstRunPageViewModel(App app, List<UpdateIntervalSettingsItem> switches, INavigation navigation)
        {
            this.app = app;
            Items = switches;
            GoToDownload = new Command(() => navigation.PushAsync(new FirstRunDownloadResourcesPage(app)));
        }

        /// <summary>
        /// Method that handles IsChecked property when the language of resources changes.
        /// </summary>
        /// <param name="sender">object that changed status</param>
        private void HandleCheckChange(UpdateIntervalSettingsItem sender)
        {
            sender.IsChecked = true;
            sender.WasUpdated = true;
            app.userSettings.UpdateInterval = UpdateintervalOptionExtensions.GetUpdateIntervalOption(sender.EnglishName);
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

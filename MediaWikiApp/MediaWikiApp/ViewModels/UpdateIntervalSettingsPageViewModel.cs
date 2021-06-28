using AppBase.Helpers;
using AppBase.Models;
using AppBaseNamespace;
using AppBaseNamespace.Models;
using AppBaseNamespace.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// Class representing the view model of the update interval settings page. 
    /// The model remembers all the interval options and languages of the resources.
    /// </summary>
    public class UpdateIntervalSettingsPageViewModel: INotifyPropertyChanged
    {
        public List<UpdateIntervalSettingsItem> Items { get; set; }
        bool statusChanged = false;
        bool isTheFirstTime = true;
        bool isOnRequest;
        public bool IsOnRequest {
            get
            {
                return isOnRequest;
            }
            set
            {
                isOnRequest = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        App app { get; set; }
        INavigation Navigation { get; set; }
        UpdateIntervalSettingsPage updateIntervalSettingsPage { get; set; }
        public UpdateIntervalSettingsPageViewModel(App app, List<UpdateIntervalSettingsItem> switches, INavigation navigation, UpdateIntervalSettingsPage page)
        {
            this.app = app;            
            Items = switches;
            IsOnRequest = app.userSettings.UpdateInterval == Models.UpdateIntervalOption.ON_REQUEST;
            Navigation = navigation;
            this.updateIntervalSettingsPage = page;
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
                IsOnRequest = app.userSettings.UpdateInterval == Models.UpdateIntervalOption.ON_REQUEST;
                statusChanged = true;
            }

            if (IsOnRequest && statusChanged && !isTheFirstTime)
            {
                statusChanged = false;
                
                Navigation.InsertPageBefore(new UpdateIntervalSettingsPage(app), updateIntervalSettingsPage);
                Navigation.PopAsync();
            }

            isTheFirstTime = false;
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

        List<LanguageSettingsItem> GetLanguages(List<string> languages)
        {
            var Languages = languages
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new LanguageSettingsItem()
                {
                    IsChecked = app.userSettings.ChosenResourceLanguages.Contains(x),
                    EnglishName = x,
                    Value = LanguagesTranslationHelper.ReturnTranslation(x),
                    WasUpdated = false,
                    Shortcut = LanguagesTranslationHelper.GetLanguageShortcut(x)
                })
                .ToList();
            return Languages;
        }

        /// <summary>
        /// Method for requesting an update of the resources.
        /// </summary>
        /// <param name="updateIntervalSettingsPage"></param>
        internal async void RequestUpdate(UpdateIntervalSettingsPage updateIntervalSettingsPage)
        {
            await RequestUpdateHelpers.RequestUpdate(updateIntervalSettingsPage, app, GetLanguages(app.userSettings.ChosenResourceLanguages));
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

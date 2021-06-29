using AppBase;
using AppBase.Helpers;
using AppBase.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using AppBaseNamespace.Models;

namespace AppBaseNamespace.ViewModels
{
    /// <summary>
    /// Class representing the view model for language settings page.
    /// </summary>
    public class LanguageSettingsPageViewModel
    {
        public List<LanguageSettingsItem> Items { get; }
        MainPageViewModel MainPageViewModelBackup { get; set; }
        App app { get; set; }
        LanguageSettingsPage SettingsPageBackup { get; set; }

        public LanguageSettingsPageViewModel(IEnumerable<string> items, IEnumerable<string> shortcuts, 
            List<string> englishVersions, LanguageSettingsPage page, MainPageViewModel mainPageViewModel, App app)
        {
            this.app = app;
            this.SettingsPageBackup = page;
            bool isFirst = true;
            MainPageViewModelBackup = mainPageViewModel;
            Items = new List<LanguageSettingsItem>();
         
            int i = 0;
            for (int j = 0; j < items.Count(); j++)            
            {
                var item = items.ElementAt(j);
                if (mainPageViewModel.previouslyChecked != Constants.EMPTY_STRING && englishVersions.Contains(
                    mainPageViewModel.previouslyChecked))
                { 
                    if (englishVersions.ElementAt(j) == mainPageViewModel.previouslyChecked)
                    {
                        AddNewItem(true, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));
                    }
                    else
                    {
                        AddNewItem(false, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));
                    }
                   
                    isFirst = false;
                }
                else
                {
                    if (isFirst)
                    {
                        AddNewItem(true, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));                        
                        isFirst = false;
                    }
                    else
                    {
                        AddNewItem(false, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));                       
                    }
                }                           
                i++;
            }                  
        }

        /// <summary>
        /// Adds new item into the Items list.
        /// </summary>
        /// <param name="isChecked">bool value representing IsChecked state (if the item was selected)</param>
        /// <param name="wasUpdated">if the item has already been updated</param>
        /// <param name="value">value being displayed</param>
        /// <param name="shortcut">shortcut of the language</param>
        /// <param name="englishName">English name of the language</param>
        private void AddNewItem(bool isChecked, bool wasUpdated, string value, string shortcut, string englishName)
        {
            Items.Add(new LanguageSettingsItem()
            {
                IsChecked = isChecked,
                WasUpdated = wasUpdated,
                Value = value,
                Shortcut = shortcut,
                EnglishName = englishName

            });
        }

        /// <summary>
        /// Method used to change the IsChecked state of the given object to false.
        /// </summary>
        /// <param name="sender">the object that is being unchecked</param>
        private void UncheckSender(LanguageSettingsItem sender)
        {
            if (!sender.WasUpdated)
            {
                sender.IsChecked = false;
                sender.NotifyPropertyChanged(Constants.IS_CHECKED_PROPERTY_NAME);
            }
        }

        /// <summary>
        /// Method used for checking the item that was previously checked and written in the Main page view model backup.
        /// </summary>
        private void CheckPreviouslyChecked()
        {
            LanguageSettingsItem previouslyChecked = Items.Find(x => (x.EnglishName == MainPageViewModelBackup.previouslyChecked));
            OnCheckBoxCheckedChanged(previouslyChecked);
            app.WasRefreshed = false;
        }      
      
        /// <summary>
        /// Method that handles checkbox IsChecked changed status asynchronously.
        /// Displays message if the user wants to change from previous language to the newly selected one.
        /// On the very first run the English option is checked by default.
        /// If user unchecked option that has been previously checked (there would be no checked item) English option is checked.
        /// </summary>
        /// <param name="checkboxSender">object that changed status</param>
        /// <returns></returns>
        public async Task OnCheckBoxCheckedChangedAsync(LanguageSettingsItem checkboxSender)
        {

            if (checkboxSender.IsChecked  && checkboxSender.EnglishName != MainPageViewModelBackup.previouslyChecked)
            {
                bool answer = await ShowPopupHelpers.ShowYesNoPopup(SettingsPageBackup,
                                                                    PopupMessageHelpers.CreatePopUpMessage(MainPageViewModelBackup.previouslyChecked, checkboxSender.EnglishName),
                                                                    PopupMessageHelpers.CreateYesMessage(MainPageViewModelBackup.previouslyChecked, checkboxSender.EnglishName),
                                                                    PopupMessageHelpers.CreateNoMessage(MainPageViewModelBackup.previouslyChecked, checkboxSender.EnglishName),
                                                                    300, 330);
                if (answer)
                {
                    OnCheckBoxCheckedChanged(checkboxSender);                    
                }
                else
                {
                    UncheckSender(checkboxSender);
                }
            }
            else if (app.WasRefreshed)
            {
                CheckPreviouslyChecked();
            }
            else
            {
                if (checkboxSender.WasUpdated == true)
                {
                    CheckAndHandleAllFalse();
                }
                else
                {
                    await CheckAndHandleMoreThanOneChecked();
                }
            }
        }

        /// <summary>
        /// Method that handles the situation where more than one would be checked. In this situation the newest is checked and rest is unchecked.
        /// </summary>
        /// <returns></returns>
        private async Task CheckAndHandleMoreThanOneChecked()
        {
            bool moreThanOneChecked = false;
            bool oneChecked = false;
            foreach (var item in Items)
            {
                if (oneChecked && item.IsChecked)
                {
                    moreThanOneChecked = true;
                    break;
                }
                else if (item.IsChecked)
                {
                    oneChecked = true;
                }
            }

            if (moreThanOneChecked)
            {
                LanguageSettingsItem previouslyChecked = Items.Find(x => (x.EnglishName == MainPageViewModelBackup.previouslyChecked));
                previouslyChecked.IsChecked = true;
                await OnCheckBoxCheckedChangedAsync(previouslyChecked);
            }
        }

        /// <summary>
        /// Method that handles the situation that no checkbox is checked. If this situation happens, English option is checked.
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
                LanguageSettingsItem english = Items.Find(x => (x.EnglishName == Constants.ENGLISH_LANGUAGE_NAME));
                english.IsChecked = true;
                english.WasUpdated = true;
                english.NotifyPropertyChanged(Constants.IS_CHECKED_PROPERTY_NAME);
            }
        }

        /// <summary>
        /// Method that enables checkbox being checked via click on the label next to it.
        /// </summary>
        /// <param name="sender">Label that was clicked on</param>
        /// <param name="e">event args</param>
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Label label = (sender as Label);
            foreach (var item in Items)
            {
                if (label.Text == item.Value)
                {
                    item.IsChecked = true;                    
                    break;
                }
            }
        }
        /// <summary>
        /// Method that handles IsChecked changed status for the sender object.
        /// </summary>
        /// <param name="sender">sender that changed status</param>
        private void HandleCheckChange(LanguageSettingsItem sender)
        {
            sender.IsChecked = true;
            sender.WasUpdated = true;
            foreach (var item in Items)
            {
                if (!item.Equals(sender))
                {
                    item.IsChecked = false;
                    item.WasUpdated = false;
                    item.NotifyPropertyChanged(Constants.IS_CHECKED_PROPERTY_NAME);
                }
            }
        }

        /// <summary>
        /// Method that handles change of the application language. All necessary information in application are set to the new language and 
        /// multilingual app resources' culture changes to the new language.
        /// In the end the application is refreshed.
        /// </summary>
        /// <param name="sender"></param>
        private void HandleLanguageChange(LanguageSettingsItem sender)
        {
            MainPageViewModelBackup.previouslyChecked = sender.EnglishName;
            CultureInfo language = new CultureInfo(sender.Shortcut);
            Thread.CurrentThread.CurrentUICulture = language;
            CultureInfo.CurrentUICulture = language;
            AppResources.Culture = language;
            if (!app.WasRefreshed) app.ReloadApp(sender.Shortcut, MainPageViewModelBackup.previouslyChecked);
        }

        /// <summary>
        /// Method that handles checkbox status change. If the sender has already been updated the situation 
        /// for all checkboxes being unchecked needs to be handled first. Then the check change of the sender can be handled and if the language changed
        /// the method for setting language of the application needs to be called.
        /// </summary>
        /// <param name="sender"></param>
        public void OnCheckBoxCheckedChanged(LanguageSettingsItem sender)
        {
            if (sender.WasUpdated == true)
            {
                CheckAndHandleAllFalse();
                return;
            }

            HandleCheckChange(sender);

            if (sender.IsChecked && sender.Value != MainPageViewModelBackup.previouslyChecked)
            {
                HandleLanguageChange(sender);
            }            
        }
    }
}

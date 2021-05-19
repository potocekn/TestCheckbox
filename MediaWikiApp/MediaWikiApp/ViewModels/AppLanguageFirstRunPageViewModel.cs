using AppBaseNamespace;
using AppBaseNamespace.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// Class representing view model of the page that is shown on the very first run of the application where user decides what language of application will be used.
    /// </summary>
    public class AppLanguageFirstRunPageViewModel
    {
        public List<LanguageSettingsItem> Items { get; }
        App app { get; set; }
        public Command GoToNextPage { get; set; }

        public AppLanguageFirstRunPageViewModel(App app, INavigation navigation, IEnumerable<string> items, IEnumerable<string> shortcuts, List<string> englishVersions)
        {
            this.app = app;
            
            GoToNextPage = new Command(() => {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    app.IsFirst = false;
                    app.availableLanguages = Helpers.UpdateSyncHelpers.DownloadLanguages(app.URL);
                    app.SaveLanguages();
                    navigation.PushAsync(new ResourceLanguagesFirstRunPage(app, app.availableLanguages));
                }
            });
            Items = CreateItems(items,shortcuts,englishVersions);
        }

        /// <summary>
        /// Method that creates list of Items that should be displayed in the page.
        /// </summary>
        /// <param name="items">list of languages</param>
        /// <param name="shortcuts">list of shortcuts of languages</param>
        /// <param name="englishVersions">list of english names of languages</param>
        /// <returns>list of LanguageSettingsItems that should be displayed</returns>
        private List<LanguageSettingsItem> CreateItems(IEnumerable<string> items, IEnumerable<string> shortcuts, List<string> englishVersions)
        {
            bool isFirst = true;
            var result = new List<LanguageSettingsItem>();
            int i = 0;
            for (int j = 0; j < items.Count(); j++)
            {
                var item = items.ElementAt(j);
                if (isFirst)
                {
                    AddNewItem(result, true, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));
                    isFirst = false;
                }
                else
                {
                    AddNewItem(result, false, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));
                }
                i++;
            }
            return result;
        }

        /// <summary>
        /// Adds new LanguageSettingsItem into the given list.
        /// </summary>
        /// <param name="whereToAdd">list where the item should be added</param>
        /// <param name="isChecked">if the checkbox next to the language label should be checked</param>
        /// <param name="wasUpdated">if the checkbox was updated</param>
        /// <param name="value">string representation of what language should be displayed</param>
        /// <param name="shortcut">shortcut of the language that should be displayed</param>
        /// <param name="englishName">english name for the language that should be displayed</param>
        private void AddNewItem(List<LanguageSettingsItem> whereToAdd, bool isChecked, bool wasUpdated, string value, string shortcut, string englishName)
        {
            whereToAdd.Add(new LanguageSettingsItem()
            {
                IsChecked = isChecked,
                WasUpdated = wasUpdated,
                Value = value,
                Shortcut = shortcut,
                EnglishName = englishName

            });
        }

        /// <summary>
        /// Method that ensures if label was clicked on, checkbox next to it will be updated
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
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
        /// Methos that handles situation, that checkbox IsChecked status changed. Method has to check if there is something checked in the list.
        /// If not, it needs to do some additional checking.
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
        }

        /// <summary>
        /// Handles the situation when all checkboxes in the list are not checked. In that case checks item containing English.
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
                LanguageSettingsItem english = Items.Find(x => (x.EnglishName == "English"));
                english.IsChecked = true;
                english.WasUpdated = true;
                english.NotifyPropertyChanged("IsChecked");
            }
        }

        /// <summary>
        /// Method that handles check change for the checkbox.
        /// </summary>
        /// <param name="sender">sender object in form of LanguageSettingsItemViewModel</param>
        private void HandleCheckChange(LanguageSettingsItem sender)
        {
            sender.IsChecked = true;
            sender.WasUpdated = true;
            app.userSettings.AppLanguage = sender.EnglishName;
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
    }
}

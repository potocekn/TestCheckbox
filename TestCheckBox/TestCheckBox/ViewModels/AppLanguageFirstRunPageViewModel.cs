using AppBaseNamespace;
using AppBaseNamespace.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    public class AppLanguageFirstRunPageViewModel
    {
        public List<LanguageSettingsItemViewModel> Items { get; }
        App app { get; set; }
        public Command GoToNextPage { get; set; }

        public AppLanguageFirstRunPageViewModel(App app, INavigation navigation, IEnumerable<string> items, IEnumerable<string> shortcuts, List<string> englishVersions)
        {
            this.app = app;
            bool isFirst = true;
            GoToNextPage = new Command(() => {
                navigation.PushAsync(new ResourceLanguagesFirstRunPage(app)); ////////////////////////////////////////////////////////////////////////////////////////////////////////
            });
            Items = new List<LanguageSettingsItemViewModel>();
            int i = 0;
            for (int j = 0; j < items.Count(); j++)
            {
                var item = items.ElementAt(j);
                if (isFirst)
                {
                    AddNewItem(true, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));
                    isFirst = false;
                }
                else
                {
                    AddNewItem(false, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));
                }
                i++;
            }
        }

        private void AddNewItem(bool isChecked, bool wasUpdated, string value, string shortcut, string englishName)
        {
            Items.Add(new LanguageSettingsItemViewModel()
            {
                IsChecked = isChecked,
                WasUpdated = wasUpdated,
                Value = value,
                Shortcut = shortcut,
                EnglishName = englishName

            });
        }

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

        public void OnCheckBoxCheckedChanged(LanguageSettingsItemViewModel sender)
        {
            if (sender.WasUpdated == true)
            {
                CheckAndHandleAllFalse();
                return;
            }

            HandleCheckChange(sender);            
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
                LanguageSettingsItemViewModel english = Items.Find(x => (x.EnglishName == "English"));
                english.IsChecked = true;
                english.WasUpdated = true;
                english.NotifyPropertyChanged("IsChecked");
            }
        }

        private void HandleCheckChange(LanguageSettingsItemViewModel sender)
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

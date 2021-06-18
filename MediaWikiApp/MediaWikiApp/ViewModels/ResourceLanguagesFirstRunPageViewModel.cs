using AppBase.Helpers;
using AppBaseNamespace;
using AppBaseNamespace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// Class representing view model of the resource languages that is displayed on the very forst run of the application.
    /// </summary>
    class ResourceLanguagesFirstRunPageViewModel
    {
        public List<LanguageSettingsItem> Languages { get; set; }
        App app;
        public Command GoToNextPage { get; set; }

        public ResourceLanguagesFirstRunPageViewModel(App app, INavigation navigation, List<string> languages)
        {
            this.app = app;
            Languages = languages
               .Where(x => !string.IsNullOrEmpty(x))
               .Select(x => new LanguageSettingsItem()
               {
                   IsChecked = false,
                   Value = LanguagesTranslationHelper.ReturnTranslation(x),
                   EnglishName = x,
                   WasUpdated = false                   
               })
               .ToList();
            GoToNextPage = new Command(() => {
                navigation.PushAsync(new ResourcesFormatFirstRunPage(app));
            });
        }

        /// <summary>
        /// Method that handles checkbox IsChecked change for specified sender.
        /// </summary>
        /// <param name="sender">checkbox that changes status</param>
        /// <param name="e">event args</param>
        public void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            foreach (var item in Languages)
            {
                if (item.EnglishName == ((sender as CheckBox).BindingContext as LanguageSettingsItem).EnglishName)
                {
                    if ((sender as CheckBox).IsChecked && !app.userSettings.ChosenResourceLanguages.Contains(item.EnglishName))
                    {
                        app.userSettings.ChosenResourceLanguages.Add(item.EnglishName);
                        app.SaveUserSettings();
                        break;
                    }
                    else if (!(sender as CheckBox).IsChecked && app.userSettings.ChosenResourceLanguages.Contains(item.EnglishName))
                    {
                        app.userSettings.ChosenResourceLanguages.Remove(item.EnglishName);
                        app.SaveUserSettings();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Method that ensures if label was clicked on, checkbox next to it will be updated
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Label label = (sender as Label);
            foreach (var item in Languages)
            {
                if (label.Text == item.EnglishName)
                {
                    item.IsChecked = !item.IsChecked;
                    break;
                }
            }
        }
    }
}

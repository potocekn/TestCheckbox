using AppBase.Helpers;
using AppBaseNamespace;
using AppBaseNamespace.ViewModels;
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
        public List<CheckBoxItem> Languages { get; set; }
        App app;
        public Command GoToNextPage { get; set; }

        public ResourceLanguagesFirstRunPageViewModel(App app, INavigation navigation, List<string> languages)
        {
            this.app = app;
            Languages = languages
               .Where(x => !string.IsNullOrEmpty(x))
               .Select(x => new CheckBoxItem()
               {
                   IsChecked = false,
                   Value = x,
                   LabelText = LanguagesTranslationHelper.ReturnTranslation(x),
                   CheckedChangedCommand = new Command(() => {
                   })
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
                if (item.Value == ((sender as CheckBox).BindingContext as CheckBoxItem).Value)
                {
                    if ((sender as CheckBox).IsChecked && !app.userSettings.ChosenResourceLanguages.Contains(item.Value))
                    {
                        app.userSettings.ChosenResourceLanguages.Add(item.Value);
                        app.SaveUserSettings();
                        break;
                    }
                    else if (!(sender as CheckBox).IsChecked && app.userSettings.ChosenResourceLanguages.Contains(item.Value))
                    {
                        app.userSettings.ChosenResourceLanguages.Remove(item.Value);
                        app.SaveUserSettings();
                        break;
                    }
                }
            }
        }
    }
}

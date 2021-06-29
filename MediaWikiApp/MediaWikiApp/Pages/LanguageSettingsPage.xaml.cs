using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBaseNamespace.ViewModels;
using AppBaseNamespace.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppBase.Helpers;

namespace AppBaseNamespace
{
    /// <summary>
    /// Class that is used to display the list of settings. 
    /// The list includes application language settings, format and language settings of resources
    /// and the update interval settings.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguageSettingsPage : ContentPage
    {
        List<string> englishVersions = new List<string>() { Constants.ENGLISH_LANGUAGE_NAME,
                Constants.GERMAN_LANGUAGE_NAME, Constants.CZECH_LANGUAGE_NAME,
                Constants.FRENCH_LANGUAGE_NAME, Constants.CHINESE_LANGUAGE_NAME };
        public LanguageSettingsPage( App app, MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            List<string> items = new List<string>();
            List<string> shortcuts = new List<string>();
            items.Add(englishLabel.Text);
            shortcuts.Add(Constants.ENGLISH_LANGUAGE_SHORTCUT);
            items.Add(germanLabel.Text);
            shortcuts.Add(Constants.GERMAN_LANGUAGE_SHORTCUT);
            items.Add(czechLabel.Text);
            shortcuts.Add(Constants.CZECH_LANGUAGE_SHORTCUT);
            items.Add(frenchLabel.Text);
            shortcuts.Add(Constants.FRENCH_LANGUAGE_SHORTCUT);
            items.Add(chineseLabel.Text);
            shortcuts.Add(Constants.CHINESE_LANGUAGE_SHORTCUT);

            BindingContext = new LanguageSettingsPageViewModel(items, shortcuts, englishVersions, this, mainPageViewModel, app);
        }

        /// <summary>
        /// Method used when the checkbox checked sattus changed.
        /// </summary>
        /// <param name="sender">Checkbox that changed status</param>
        /// <param name="e">Event arguments</param>
        async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            await ((LanguageSettingsPageViewModel)BindingContext).OnCheckBoxCheckedChangedAsync((
                (sender as CheckBox).BindingContext as LanguageSettingsItem));

        }
        
        /// <summary>
        /// Method used for checking checkbox when label next to it is tapped.
        /// </summary>
        /// <param name="sender">Label that was clicked on</param>
        /// <param name="e">Event arguments</param>
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ((LanguageSettingsPageViewModel)BindingContext).TapGestureRecognizer_Tapped(sender, e);
        }      
    }
}
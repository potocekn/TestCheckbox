using AppBase.ViewModels;
using AppBaseNamespace;
using AppBaseNamespace.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBase
{
    /// <summary>
    /// Class used for the configuration process of the application. 
    /// This class displays the list of available languages of the application and the instructions for the user.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppLanguageFirstRunPage : ContentPage
    {

        public AppLanguageFirstRunPage(App app)
        {
            InitializeComponent();
            List<string> englishVersions = new List<string>() { "English", "German", "Czech", "French", "Chinese" };
            List<string> items = new List<string>();
            List<string> shortcuts = new List<string>();
            items.Add(englishLabel.Text);
            shortcuts.Add("en");
            items.Add(germanLabel.Text);
            shortcuts.Add("de");
            items.Add(czechLabel.Text);
            shortcuts.Add("cs");
            items.Add(frenchLabel.Text);
            shortcuts.Add("fr");
            items.Add(chineseLabel.Text);
            shortcuts.Add("zh-Hans");

            BindingContext = new AppLanguageFirstRunPageViewModel(app, this, Navigation, items, shortcuts, englishVersions);
        }
        
        /// <summary>
        /// Method for the checkbox to call on the checked status change.
        /// </summary>
        /// <param name="sender">checkbox that changed status</param>
        /// <param name="e">Event arguments</param>
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ((AppLanguageFirstRunPageViewModel)BindingContext).OnCheckBoxCheckedChanged(((sender as CheckBox).BindingContext as LanguageSettingsItem));

        }

        /// <summary>
        /// Method used to check checkbox by clicking on the label next to the checbox.
        /// </summary>
        /// <param name="sender">Label that was clicked on</param>
        /// <param name="e">Event arguments</param>
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ((AppLanguageFirstRunPageViewModel)BindingContext).TapGestureRecognizer_Tapped(sender, e);
        }
    }

}
using AppBase.Models;
using AppBase.ViewModels;
using AppBaseNamespace;
using AppBaseNamespace.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBase
{
    /// <summary>
    /// Class representing the page that provides update interval settings in the settings menu. 
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateIntervalSettingsPage : ContentPage
    {
        List<UpdateIntervalSettingsItem> Checkboxes = new List<UpdateIntervalSettingsItem>();
        public UpdateIntervalSettingsPage(App app)
        {
            InitializeComponent();

            if (app.userSettings.UpdateInterval == Models.UpdateIntervalOption.AUTOMATIC)
            {
                AddItem(AutomaticOptionLabel.Text, true, false, englishAutomatic.Text);
            }
            else
            {
                AddItem(AutomaticOptionLabel.Text, false, false, englishAutomatic.Text);
            }

            if (app.userSettings.UpdateInterval == Models.UpdateIntervalOption.ONCE_A_MONTH)
            {
                AddItem(OnceAMonthOptionLabel.Text, true, false, englishOnceAMonth.Text);
            }
            else
            {
                AddItem(OnceAMonthOptionLabel.Text, false, false, englishOnceAMonth.Text);
            }

            if (app.userSettings.UpdateInterval == Models.UpdateIntervalOption.ON_REQUEST)
            {
                AddItem(OnRequestOptionLabel.Text, true, false, englishOnRequest.Text);
            }
            else
            {
                AddItem(OnRequestOptionLabel.Text, false, false, englishOnRequest.Text);
            }
            BindingContext = new UpdateIntervalSettingsPageViewModel(app, Checkboxes, Navigation, this);
        }

        /// <summary>
        /// Method used for adding new item into list of UpdateIntervalSettingsItem items.
        /// </summary>
        /// <param name="name">Name of the checkbox (type of update interval option that this item represents)</param>
        /// <param name="isChecked">If the corresponding checkbox is checked</param>
        /// <param name="wasUpdated">If the checkbox was updated</param>
        /// <param name="englishName">English version of the item name</param>
        void AddItem(string name, bool isChecked, bool wasUpdated, string englishName)
        {
            Checkboxes.Add(new UpdateIntervalSettingsItem()
            {
                Name = name,
                WasUpdated = wasUpdated,
                IsChecked = isChecked, 
                EnglishName = englishName
            });
        }

        /// <summary>
        /// Method used when a checkbox changes its checked status.
        /// </summary>
        /// <param name="sender">Checkbox that changed its status</param>
        /// <param name="e">Event arguments</param>
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            (BindingContext as UpdateIntervalSettingsPageViewModel).OnCheckedChanged(((sender as CheckBox).BindingContext as UpdateIntervalSettingsItem));
        }

        /// <summary>
        /// Method used for checking a checkbox when the label next to it was clicked on.
        /// </summary>
        /// <param name="sender">Label that was clicked on</param>
        /// <param name="e">event arguments</param>
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            (BindingContext as UpdateIntervalSettingsPageViewModel).TapGestureRecognizer_Tapped(sender, e);
        }

        /// <summary>
        /// Method used when requesting an update.
        /// </summary>
        /// <param name="sender">Request button that was clicked on</param>
        /// <param name="e">Event arguments</param>
        private void RequestUpdateButton_Clicked(object sender, EventArgs e)
        {
            (BindingContext as UpdateIntervalSettingsPageViewModel).RequestUpdate(this);
        }
    }
}
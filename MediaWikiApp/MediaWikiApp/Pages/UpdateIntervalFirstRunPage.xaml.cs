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

namespace AppBase.Pages
{
    /// <summary>
    /// Class that is used during the first configuration process. 
    /// It is responsible for selecting the update interval of the resources.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateIntervalFirstRunPage : ContentPage
    {
        List<UpdateIntervalSettingsItem> Checkboxes = new List<UpdateIntervalSettingsItem>();
        public UpdateIntervalFirstRunPage(App app)
        {
            InitializeComponent();
            
            AddItem(AutomaticOptionLabel.Text, true, false, englishAutomatic.Text);           
            AddItem(OnceAMonthOptionLabel.Text, false, false, englishOnceAMonth.Text);           
            AddItem(OnRequestOptionLabel.Text, false, false, englishOnRequest.Text);           

            BindingContext = new UpdateIntervalFirstRunPageViewModel(app, Checkboxes, Navigation);
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
            var checkbox = ((sender as CheckBox).BindingContext as UpdateIntervalSettingsItem);
            (BindingContext as UpdateIntervalFirstRunPageViewModel).OnCheckedChanged(checkbox);
        }

        /// <summary>
        /// Method used for checking a checkbox when the label next to it was clicked on.
        /// </summary>
        /// <param name="sender">Label that was clicked on</param>
        /// <param name="e">event arguments</param>
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            (BindingContext as UpdateIntervalFirstRunPageViewModel).TapGestureRecognizer_Tapped(sender, e);
        }
    }
}
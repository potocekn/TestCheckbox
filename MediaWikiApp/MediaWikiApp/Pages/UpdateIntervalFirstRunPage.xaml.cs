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
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = ((sender as CheckBox).BindingContext as UpdateIntervalSettingsItem);
            (BindingContext as UpdateIntervalFirstRunPageViewModel).OnCheckedChanged(checkbox);
        }
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            (BindingContext as UpdateIntervalFirstRunPageViewModel).TapGestureRecognizer_Tapped(sender, e);
        }
    }
}
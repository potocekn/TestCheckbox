using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TestCheckbox.ViewModels
{
    public class MainPageViewModel
    {
        private string[] textCollection = new string[] { "English", "Czech", "German"};
        public string Title { get; }
        public Command LoadCheckboxes { get; }

        public MainPageViewModel(Page page, INavigation navigation)
        {
            LoadCheckboxes = new Command(() => {
                navigation.PushAsync(new CheckPage(textCollection));
            });
        }
    }
}

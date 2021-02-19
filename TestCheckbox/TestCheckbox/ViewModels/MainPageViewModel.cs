using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TestCheckbox.ViewModels
{
    public class MainPageViewModel
    {
        private string[] textCollection = new string[] { "English", "Czech", "German"};
        private string[] languages = new string[] { "English", "German", "Czech", "French", "Chinese"};
        public string Title { get; }
        public Command LoadCheckboxes { get; }
        public Command GoToSettings { get; }

        public MainPageViewModel(Page page, App app,INavigation navigation)
        {
            LoadCheckboxes = new Command(() => {
                navigation.PushAsync(new CheckPage(textCollection));
            });
            GoToSettings = new Command(() => {
                navigation.PushAsync(new SettingsPage(languages));
            });
        }
    }
}

using AppBaseNamespace;
using AppBaseNamespace.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{    
    /// <summary>
    /// Class that represents the view model of the settings page. The model contains commands that go to different settings options
    /// such as app language change, change of the update interval and change of the resources formats.
    /// </summary>
    public class SettingsPageViewModel
    {
        public Command GoToLanguageSettings { get; }
        public Command GoToUpdateIntervalSettings { get; }
        public Command GoToResourceFormatSettings { get; }
        public Command GoToFileLocationSettings { get; }


        public SettingsPageViewModel(App app, INavigation navigation, MainPageViewModel mainPageViewModel)
        {
            GoToLanguageSettings = new Command(() => {
                navigation.PushAsync(new LanguageSettingsPage(app, mainPageViewModel));
            });
            GoToUpdateIntervalSettings = new Command(() => {
                navigation.PushAsync(new UpdateIntervalSettingsPage(app));
            });
            GoToResourceFormatSettings = new Command(() => {
                navigation.PushAsync(new ResourceFormatSettingsPage(app, mainPageViewModel));
            });            
        }
    }
}

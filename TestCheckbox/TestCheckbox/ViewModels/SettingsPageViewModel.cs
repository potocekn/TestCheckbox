using AppBaseNamespace;
using AppBaseNamespace.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{    
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
                navigation.PushAsync(new UpdateIntervalSettingsPage(app, mainPageViewModel));
            });
            GoToResourceFormatSettings = new Command(() => {
                navigation.PushAsync(new ResourceFormatSettingsPage(app, mainPageViewModel));
            });
            GoToFileLocationSettings = new Command(() => {
                navigation.PushAsync(new FileLocationSettingsPage(app, mainPageViewModel));
            });
        }
    }
}

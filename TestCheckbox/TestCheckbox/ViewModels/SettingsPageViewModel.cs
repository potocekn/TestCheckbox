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

        public SettingsPageViewModel(App app, INavigation navigation, MainPageViewModel mainPageViewModel)
        {
            GoToLanguageSettings = new Command(() => {
                navigation.PushAsync(new LanguageSettingsPage(app, mainPageViewModel));
            });
        }
    }
}

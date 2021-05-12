using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Xamarin.Forms;
using AppBase;

namespace AppBaseNamespace.ViewModels
{
    /// <summary>
    /// Class representing the view model of the main page. 
    /// Class remembers its title and has commands that redirect main page to settings or resources.
    /// </summary>
    public class MainPageViewModel
    {
        private List<string> textCollection = new List<string> { "English", "Czech", "German"};
        private List<string> languages = new List<string> {  "Czech", "French", "Chinese"};
        private List<string> shortcuts = new List<string> {  "cs", "fr", "zh-Hans"};
        public string previouslyChecked = "";

        List<string> ResourceLanguages { get; set; }
        public string Title { get; }
        public Command LoadCheckboxes { get; }
        public Command GoToSettings { get; }
        public Command GoToResources { get; }

        public MainPageViewModel(Page page, App app,INavigation navigation, string previouslyChecked)
        {
            this.previouslyChecked = previouslyChecked;
            LoadCheckboxes = new Command(() => {
                navigation.PushAsync(new ResourceFormatSettingsPage(app, this));
            });
            GoToSettings = new Command(() => {
                navigation.PushAsync(new SettingsPage(app, this));
            });
            GoToResources = new Command(() => {
                navigation.PushAsync(new ResourcesPage(app, navigation));
            });

        } 
    }
}

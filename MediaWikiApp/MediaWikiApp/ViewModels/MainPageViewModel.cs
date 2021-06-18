using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Xamarin.Forms;
using AppBase;
using AppBase.Models;
using AppBase.UserSettingsHelpers;
using System.Globalization;

namespace AppBaseNamespace.ViewModels
{
    /// <summary>
    /// Class representing the view model of the main page. 
    /// Class remembers its title and has commands that redirect main page to settings or resources.
    /// </summary>
    public class MainPageViewModel
    {        
        public string previouslyChecked = "";

        public List<ResourceLanguageInfo> ResourceLanguages { get; set; }
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
            ResourceLanguages = SeparateLanguages(app, navigation);
        }

        /// <summary>
        /// Method used for separating resources for each language. On the main page each language has a separate button 
        /// so that when user wants to access only one language it would be more convenient.
        /// </summary>
        /// <param name="app">The referrence to the current app.</param>
        /// <param name="navigation">The current navigation bar - needed for pushing new pages on the navigation.</param>
        /// <returns>List of separated resources for each language.</returns>
        List<ResourceLanguageInfo> SeparateLanguages(App app, INavigation navigation)
        {
            List<ResourceLanguageInfo> result = new List<ResourceLanguageInfo>();
            foreach (var language in app.userSettings.ChosenResourceLanguages)
            {
                ResourceLanguageInfo resourceLanguageInfo = new ResourceLanguageInfo();

                resourceLanguageInfo.LanguageName = AppBase.Helpers.LanguagesTranslationHelper.ReturnTranslation(language);
                resourceLanguageInfo.PDFs = SeparatePDFsForLanguage(language, app);
                resourceLanguageInfo.ODTs = SeparateODTsForLanguage(language, app);
                resourceLanguageInfo.HTMLs = SeparateHTMLsForLanguage(language);
                resourceLanguageInfo.OpenResources = new Command(() =>
                {
                    navigation.PushAsync(new ResourcesPage(resourceLanguageInfo, navigation));
                });
                
                result.Add(resourceLanguageInfo);
            }
            return result;
        }

        /// <summary>
        /// Method for separating the PDF files for given language.
        /// </summary>
        /// <param name="language">Language for which the separation should be done.</param>
        /// <param name="app">The referrence to the current app.</param>
        /// <returns>List of separated PDF files.</returns>
        List<ResourcesInfoPDF> SeparatePDFsForLanguage(string language, App app)
        {
            List<ResourcesInfoPDF> result = new List<ResourcesInfoPDF>();

            foreach (var item in app.resourcesPDF)
            {
                if (item.Language == language)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for separating the ODT files for given language.
        /// </summary>
        /// <param name="language">Language for which the separation should be done.</param>
        /// <param name="app">The referrence to the current app.</param>
        /// <returns>List of separated ODT files.</returns>
        List<ResourcesInfoPDF> SeparateODTsForLanguage(string language, App app)
        {
            List<ResourcesInfoPDF> result = new List<ResourcesInfoPDF>();

            foreach (var item in app.resourcesODT)
            {
                if (item.Language == language)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for separating the HTML files for given language
        /// </summary>
        /// <param name="language">Language for which the separation should be done.</param>
        /// <returns></returns>
        List<HtmlRecord> SeparateHTMLsForLanguage(string language)
        {
            var allPages = App.Database.GetPagesAsync().Result;
            List<HtmlRecord> result = allPages.FindAll(x => {
                CultureInfo ci = new CultureInfo(x.PageLanguage);
                return ci.DisplayName == language;
                });            
            return result;
        }
    }
}

using AppBase;
using AppBase.Helpers;
using AppBase.Resources;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestCheckbox.ViewModels
{
    public class SettingsPageViewModel
    {
        public List<SettingsItemViewModel> Items { get; }
        MainPageViewModel MainPageViewModelBackup { get; set; }
        App app { get; set; }
        SettingsPage SettingsPageBackup { get; set; }

        public SettingsPageViewModel(IEnumerable<string> items, IEnumerable<string> shortcuts, List<string> englishVersions, SettingsPage page, MainPageViewModel mainPageViewModel, App app)
        {
            this.app = app;
            this.SettingsPageBackup = page;
            bool isFirst = true;
            MainPageViewModelBackup = mainPageViewModel;
            Items = new List<SettingsItemViewModel>();
         
            int i = 0;
            for (int j = 0; j < items.Count(); j++)            
            {
                var item = items.ElementAt(j);
                if (mainPageViewModel.previouslyChecked != "" && englishVersions.Contains(mainPageViewModel.previouslyChecked))
                { 
                    if (englishVersions.ElementAt(j) == mainPageViewModel.previouslyChecked)
                    {
                        AddNewItem(true, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));
                    }
                    else
                    {
                        AddNewItem(false, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));
                    }
                   
                    isFirst = false;
                }
                else
                {
                    if (isFirst)
                    {
                        AddNewItem(true, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));                        
                        isFirst = false;
                    }
                    else
                    {
                        AddNewItem(false, false, item, shortcuts.ElementAt(i), englishVersions.ElementAt(i));                       
                    }
                }                           
                i++;
            }                  
        }

        private void AddNewItem(bool isChecked, bool wasUpdated, string value, string shortcut, string englishName)
        {
            Items.Add(new SettingsItemViewModel()
            {
                IsChecked = isChecked,
                WasUpdated = wasUpdated,
                Value = value,
                Shortcut = shortcut,
                EnglishName = englishName

            });
        }

        private void UncheckSender(SettingsItemViewModel sender)
        {
            if (!sender.WasUpdated)
            {
                sender.IsChecked = false;
                sender.NotifyPropertyChanged("IsChecked");
            }
        }

        private void CheckPreviouslyChecked()
        {
            SettingsItemViewModel previouslyChecked = Items.Find(x => (x.EnglishName == MainPageViewModelBackup.previouslyChecked));
            OnCheckBoxCheckedChanged(previouslyChecked);
            app.WasRefreshed = false;
        }      
      

        public async Task OnCheckBoxCheckedChangedAsync(SettingsItemViewModel checkboxSender)
        {

            if (checkboxSender.IsChecked && !app.IsFirst && !app.WasRefreshed && checkboxSender.EnglishName != MainPageViewModelBackup.previouslyChecked)
            {
                bool answer = await SettingsPageBackup.DisplayAlert("", PopupMessageHelpers.CreatePopUpMessage(MainPageViewModelBackup.previouslyChecked, checkboxSender.EnglishName),
                                                                        PopupMessageHelpers.CreateYesMessage(MainPageViewModelBackup.previouslyChecked, checkboxSender.EnglishName),
                                                                        PopupMessageHelpers.CreateNoMessage(MainPageViewModelBackup.previouslyChecked, checkboxSender.EnglishName));
                if (answer)
                {
                    OnCheckBoxCheckedChanged(checkboxSender);                    
                }
                else
                {
                    UncheckSender(checkboxSender);
                }
            }
            else if (app.WasRefreshed)
            {
                CheckPreviouslyChecked();
            }
            else if (app.IsFirst)
            {
                CheckSender(checkboxSender);
                app.IsFirst = false;
            }
            else if (!app.IsFirst && !checkboxSender.IsChecked && checkboxSender.EnglishName == MainPageViewModelBackup.previouslyChecked)
            {
                CheckSender(checkboxSender);
            }
            else
            {
                if (checkboxSender.WasUpdated == true)
                {
                    CheckAndHandleAllFalse();
                }
                else
                {
                    await CheckAndHandleMoreThanOneChecked();
                }
            }
        }

        private void CheckSender(SettingsItemViewModel sender)
        {
            sender.IsChecked = true;
            sender.WasUpdated = true;
        }

        private async Task CheckAndHandleMoreThanOneChecked()
        {
            bool moreThanOneChecked = false;
            bool oneChecked = false;
            foreach (var item in Items)
            {
                if (oneChecked && item.IsChecked)
                {
                    moreThanOneChecked = true;
                    break;
                }
                else if (item.IsChecked)
                {
                    oneChecked = true;
                }
            }

            if (moreThanOneChecked)
            {
                SettingsItemViewModel previouslyChecked = Items.Find(x => (x.EnglishName == MainPageViewModelBackup.previouslyChecked));
                previouslyChecked.IsChecked = true;
                await OnCheckBoxCheckedChangedAsync(previouslyChecked);
            }
        }
        private void CheckAndHandleAllFalse()
        {
            bool allFalse = true;

            foreach (var item in Items)
            {
                if (item.IsChecked)
                {
                    allFalse = false;
                    break;
                }
            }

            if (allFalse)
            {
                SettingsItemViewModel english = Items.Find(x => (x.EnglishName == "English"));
                english.IsChecked = true;
                english.WasUpdated = true;
                english.NotifyPropertyChanged("IsChecked");
            }
        }
        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Label label = (sender as Label);
            foreach (var item in Items)
            {
                if (label.Text == item.Value)
                {
                    item.IsChecked = true;                    
                    break;
                }
            }
        }

        public async Task CheckCheckboxfromLabelClick(Label label)
        {
            foreach (var item in Items)
            {
                if (label.Text == item.Value)
                {                    
                   await OnCheckBoxCheckedChangedAsync(item);                    
                }
            }
        }

        private void HandleCheckChange(SettingsItemViewModel sender)
        {
            sender.IsChecked = true;
            sender.WasUpdated = true;
            foreach (var item in Items)
            {
                if (!item.Equals(sender))
                {
                    item.IsChecked = false;
                    item.WasUpdated = false;
                    item.NotifyPropertyChanged("IsChecked");
                }
            }
        }

        private void HandleLanguageChange(SettingsItemViewModel sender)
        {
            MainPageViewModelBackup.previouslyChecked = sender.EnglishName;
            CultureInfo language = new CultureInfo(sender.Shortcut);
            Thread.CurrentThread.CurrentUICulture = language;
            CultureInfo.CurrentUICulture = language;
            AppResources.Culture = language;
            if (!app.WasRefreshed) app.ReloadApp(sender.Shortcut, MainPageViewModelBackup.previouslyChecked);
        }

        public void OnCheckBoxCheckedChanged(SettingsItemViewModel sender)
        {
            if (sender.WasUpdated == true)
            {
                CheckAndHandleAllFalse();
                return;
            }

            HandleCheckChange(sender);

            if (sender.IsChecked && sender.Value != MainPageViewModelBackup.previouslyChecked)
            {
                HandleLanguageChange(sender);
            }            
        }
    }
}

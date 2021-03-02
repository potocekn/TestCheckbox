using AppBase.Resources;
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
        public Command ChangingCheckBox { get; }
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
                        Items.Add(new SettingsItemViewModel()
                        {
                            IsChecked = true,
                            WasUpdated = false,
                            Value = item,
                            Shortcut = shortcuts.ElementAt(i),
                            EnglishName = englishVersions.ElementAt(i)
                        }) ;
                    }
                    else
                    {
                        Items.Add(new SettingsItemViewModel()
                        {
                            IsChecked = false,
                            WasUpdated = false,
                            Value = item,
                            Shortcut = shortcuts.ElementAt(i),
                            EnglishName = englishVersions.ElementAt(i)
                        });
                    }
                   
                    isFirst = false;
                }
                else
                {
                    if (isFirst)
                    {
                        Items.Add(new SettingsItemViewModel()
                        {
                            IsChecked = true,
                            WasUpdated = false,
                            Value = item,
                            Shortcut = shortcuts.ElementAt(i),
                            EnglishName = englishVersions.ElementAt(i)
                        });
                        isFirst = false;
                    }
                    else
                    {
                        Items.Add(new SettingsItemViewModel()
                        {
                            IsChecked = false,
                            WasUpdated = false,
                            Value = item,
                            Shortcut = shortcuts.ElementAt(i),
                            EnglishName = englishVersions.ElementAt(i)

                        });
                    }
                }
                
                
                i++;
            }                  
        }

        public async Task OnCheckBoxCheckedChangedAsync(SettingsItemViewModel checkboxSender)
        {

            if (checkboxSender.IsChecked && !app.IsFirst && !app.WasRefreshed && checkboxSender.EnglishName != MainPageViewModelBackup.previouslyChecked)
            {
                bool answer = await SettingsPageBackup.DisplayAlert("Language change!", "Would you like to change language of the application?", "Yes", "No");
                if (answer)
                {
                    OnCheckBoxCheckedChanged(checkboxSender);
                }
                else
                {
                    if (!checkboxSender.WasUpdated)
                    {
                        checkboxSender.IsChecked = false;
                        checkboxSender.NotifyPropertyChanged("IsChecked");
                    }

                }
            }
            else if (app.WasRefreshed)
            {
                SettingsItemViewModel previouslyChecked = Items.Find(x => (x.EnglishName == MainPageViewModelBackup.previouslyChecked));
                OnCheckBoxCheckedChanged(previouslyChecked);
                app.WasRefreshed = false;

            }
            else if (app.IsFirst)
            {
                //(sender as CheckBox).IsChecked = true;
                checkboxSender.IsChecked = true;
                checkboxSender.WasUpdated = true;
                app.IsFirst = false;
            }
            else if (!app.IsFirst && !checkboxSender.IsChecked && checkboxSender.EnglishName == MainPageViewModelBackup.previouslyChecked)
            {
                //(sender as CheckBox).IsChecked = true;
                checkboxSender.IsChecked = true;
                checkboxSender.WasUpdated = true;
            }
            else
            {
                if (checkboxSender.WasUpdated == true)
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
                        Items.Find(x => (x.EnglishName == "English")).IsChecked = true;
                        Items.Find(x => (x.EnglishName == "English")).WasUpdated = true;
                        Items.Find(x => (x.EnglishName == "English")).NotifyPropertyChanged("IsChecked");
                    }                   
                }
                else
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
            }
        }

        public async Task TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Label label = (sender as Label);
            foreach (var item in Items)
            {
                if (label.Text == item.Value)
                {
                    item.IsChecked = true;
                    await OnCheckBoxCheckedChangedAsync(item);
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

        public void OnCheckBoxCheckedChanged(SettingsItemViewModel sender)
        {
            if (sender.WasUpdated == true)
            {
                bool allFalse = true;
                foreach (var item in Items)
                {
                    if (item.IsChecked)
                    {
                        allFalse = false;
                    }
                }

                if (allFalse)
                {
                    Items.Find(x => (x.EnglishName == "English")).IsChecked = true;
                    Items.Find(x => (x.EnglishName == "English")).WasUpdated = true;
                    Items.Find(x => (x.EnglishName == "English")).NotifyPropertyChanged("IsChecked");
                }

                return;
            }

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
            if (sender.IsChecked && sender.Value != MainPageViewModelBackup.previouslyChecked)
            {
                MainPageViewModelBackup.previouslyChecked = sender.EnglishName;
                CultureInfo language = new CultureInfo(sender.Shortcut);
                Thread.CurrentThread.CurrentUICulture = language;
                CultureInfo.CurrentUICulture = language;
                //App.Current.Properties["currentLanguage"] = language;
                AppResources.Culture = language;
                if (!app.WasRefreshed) app.ReloadApp(sender.Shortcut, MainPageViewModelBackup.previouslyChecked);
            }            
        }
    }
}

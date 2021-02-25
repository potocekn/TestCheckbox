using AppBase.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace TestCheckbox.ViewModels
{
    public class SettingsPageViewModel
    {
        public List<SettingsItemViewModel> Items { get; }
        public Command ChangingCheckBox { get; }
        MainPageViewModel MainPageViewModelBackup { get; set; }
        App app { get; set; }

        public SettingsPageViewModel(IEnumerable<string> items, IEnumerable<string> shortcuts, SettingsPage page, MainPageViewModel mainPageViewModel, App app)
        {
            this.app = app;
            bool isFirst = true;
            MainPageViewModelBackup = mainPageViewModel;
            Items = new List<SettingsItemViewModel>();
         
            int i = 0;
            foreach (var item in items)
            {
                if (mainPageViewModel.previouslyChecked != "" && items.Contains(mainPageViewModel.previouslyChecked))
                { 
                    if (item == mainPageViewModel.previouslyChecked)
                    {
                        Items.Add(new SettingsItemViewModel()
                        {
                            IsChecked = true,
                            WasUpdated = false,
                            Value = item,
                            Shortcut = shortcuts.ElementAt(i)
                        });
                    }
                    else
                    {
                        Items.Add(new SettingsItemViewModel()
                        {
                            IsChecked = false,
                            WasUpdated = false,
                            Value = item,
                            Shortcut = shortcuts.ElementAt(i)
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
                            Shortcut = shortcuts.ElementAt(i)
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
                            Shortcut = shortcuts.ElementAt(i)

                        });
                    }
                }
                
                
                i++;
            }                  
        }

        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            CheckCheckboxfromLabelClick((sender as Label));            
        }

        public void CheckCheckboxfromLabelClick(Label label)
        {
            foreach (var item in Items)
            {
                if (label.Text == item.Value)
                {                    
                    OnCheckChanged(item);
                    break;
                }
            }
        }

        public void OnCheckChanged(SettingsItemViewModel sender)
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
                    Items.Find(x => (x.Value == "English")).IsChecked = true;
                    Items.Find(x => (x.Value == "English")).WasUpdated = true;
                    Items.Find(x => (x.Value == "English")).NotifyPropertyChanged("IsChecked");
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
                MainPageViewModelBackup.previouslyChecked = sender.Value;
                CultureInfo language = new CultureInfo(sender.Shortcut);
                Thread.CurrentThread.CurrentUICulture = language;
                CultureInfo.CurrentUICulture = language;
                //App.Current.Properties["currentLanguage"] = language;
                AppResources.Culture = language;
                app.ReloadApp(sender.Shortcut, MainPageViewModelBackup.previouslyChecked);
            }            
        }
    }
}

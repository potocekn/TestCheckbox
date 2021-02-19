using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace TestCheckbox.ViewModels
{
    public class SettingsPageViewModel
    {
        public List<SettingsItemViewModel> Items { get; }
        public Command ChangingCheckBox { get; }

        public SettingsPageViewModel(IEnumerable<string> items, SettingsPage page)
        {
            bool isFirst = true;
            Items = new List<SettingsItemViewModel>();
            foreach (var item in items)
            {
                if (isFirst)
                {
                    Items.Add(new SettingsItemViewModel()
                    {
                        IsChecked = true,
                        Value = item                       
                    });
                    isFirst = false;
                }
                else
                {
                    Items.Add(new SettingsItemViewModel()
                    {
                        IsChecked = false,
                        Value = item                        
                    });
                }

            }                  
        }

        public void OnCheckChanged(SettingsItemViewModel sender)
        {
            foreach (var item in Items)
            {
                if (item.Value != sender.Value)
                {
                    item.IsChecked = false;
                    item.NotifyPropertyChanged();
                }
            }
        }
    }
}

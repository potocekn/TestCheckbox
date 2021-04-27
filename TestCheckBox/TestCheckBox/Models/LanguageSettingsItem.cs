using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace AppBaseNamespace.ViewModels
{
    public class LanguageSettingsItem: INotifyPropertyChanged
    {
        
        private bool isCheck;      
        public bool IsChecked 
        {
            get
            {
                return isCheck;
            }
            set
            {
                if (value != this.isCheck)
                {
                    this.isCheck = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Shortcut { get; set; }
        public string EnglishName { get; set; }
        public bool WasUpdated { get; set; }
        public string Value { get; set; }  

        public event PropertyChangedEventHandler PropertyChanged;

        
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

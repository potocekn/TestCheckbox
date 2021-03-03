using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    public class UpdateIntervalSettingsItemViewModel : INotifyPropertyChanged
    {
        private bool isChecked;        
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                if (value != this.isChecked)
                {
                    this.isChecked = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Name { get; set; }
        public bool WasUpdated { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

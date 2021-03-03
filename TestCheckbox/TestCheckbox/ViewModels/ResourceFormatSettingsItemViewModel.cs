using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    public class ResourceFormatSettingsItemViewModel
    {
        public Switch CorrespondingSwitch { get; set; }
        public string Name { get; set; }

        public ResourceFormatSettingsItemViewModel(Switch correspondingSwitch, string name)
        {
            CorrespondingSwitch = correspondingSwitch;
            Name = name;
        }
    }
}

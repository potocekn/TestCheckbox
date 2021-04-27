using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    public class ResourceFormatSettingsItem
    {
        public Switch CorrespondingSwitch { get; set; }
        public string Name { get; set; }

        public ResourceFormatSettingsItem(Switch correspondingSwitch, string name)
        {
            CorrespondingSwitch = correspondingSwitch;
            Name = name;
        }
    }
}

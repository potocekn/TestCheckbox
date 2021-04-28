using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// Class representing item used for determining resource format settings. Each item remembers its corresponding switch object and its name.
    /// </summary>
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

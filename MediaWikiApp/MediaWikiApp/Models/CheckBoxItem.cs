using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBaseNamespace.ViewModels
{
    /// <summary>
    /// Class representing check box and the label that is next to it. It is used in lists of checkboxes for selecting language of the application
    /// or language of the resources.
    /// </summary>
    public class CheckBoxItem
    {
        public bool IsChecked { get; set; }
        public string Value { get; set; }
        public string LabelText { get; set; }
        public Command CheckedChangedCommand { get; set; }
        public CheckBox CorrespondingCheckbox { get; set; }    

    }
}

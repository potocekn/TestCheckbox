using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBaseNamespace.ViewModels
{
    public class ItemViewModel
    {
        public bool IsChecked { get; set; }
        public string Value { get; set; }
        public Command CheckedChangedCommand { get; set; }
        public CheckBox CorrespondingCheckbox { get; set; }
            

    }
}

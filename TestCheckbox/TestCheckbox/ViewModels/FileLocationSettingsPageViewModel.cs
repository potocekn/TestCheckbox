using AppBaseNamespace;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.ViewModels
{
    public class FileLocationSettingsPageViewModel
    {
        App app; 
        public FileLocationSettingsPageViewModel(App app)
        {
            this.app = app;
        }

        public async void OnClicked(object sender, EventArgs e)
        {
            
        }
    }
}

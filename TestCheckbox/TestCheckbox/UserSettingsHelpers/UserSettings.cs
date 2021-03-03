using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.UserSettingsHelpers
{
    public class UserSettings
    {
        public string ResourcesLocation { get; set; }
        public string AppLanguage { get; set; }
        public List<string> ChosenResourceLanguages { get; set; }
        public string UpdateInterval { get; set; }
        public bool DownloadOnlyWithWifi { get; set; }
        public List<string> Formats { get; set; }

        public UserSettings(string path)
        {
            ResourcesLocation = path;
            AppLanguage = "English";
            ChosenResourceLanguages = new List<string>();
            UpdateInterval = "Automatic";
            DownloadOnlyWithWifi = false;
            Formats = new List<string>();
            Formats.Add("PDF");
        }
    }
}

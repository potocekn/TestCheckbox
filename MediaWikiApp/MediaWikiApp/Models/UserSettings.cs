using AppBase.Helpers;
using AppBase.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.Models
{
    /// <summary>
    /// Class representing everything that needs to be saved about user. More precisely where the resources are stored, 
    /// what is the language of application, which resources were chosen, 
    /// update interval option, if the downloads can be done only with wifi,
    /// what format user wants to save and the date of the last update.
    /// </summary>
    public class UserSettings
    {
        public string ResourcesLocation { get; set; }
        public string AppLanguage { get; set; }
        public List<string> ChosenResourceLanguages { get; set; }
        public UpdateIntervalOption UpdateInterval { get; set; }
        public bool DownloadOnlyWithWifi { get; set; }
        public List<string> Formats { get; set; }
        public DateTime DateOfLastUpdate { get; set; }
        public bool WasFirstDownload { get; set; }

        public UserSettings(string path)
        {
            ResourcesLocation = path;
            AppLanguage = Constants.ENGLISH_LANGUAGE_NAME;
            ChosenResourceLanguages = new List<string>();
            UpdateInterval = UpdateIntervalOption.AUTOMATIC;
            DownloadOnlyWithWifi = false;
            Formats = new List<string>();
            DateOfLastUpdate = DateTime.Now;
            WasFirstDownload = false;
        }
    }
}

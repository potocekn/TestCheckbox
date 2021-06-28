using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppBase.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TestCheckbox.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDownloader))]
namespace TestCheckbox.Droid
{
    /// <summary>
    /// Custom dependency service for downloading files on Android platform.
    /// </summary>
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        /// <summary>
        /// Method used for downloading files.
        /// </summary>
        /// <param name="url">url of the file</param>
        /// <param name="folder">folder into which to store the file</param>
        /// <param name="fileName">nema ounder which to store the file</param>
        public void DownloadFile(string url, string folder, string fileName)
        {
            string pathToNewFolder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), folder);
            Directory.CreateDirectory(pathToNewFolder);

            try
            {
                WebClient webClient = new WebClient();
                string pathToNewFile = Path.Combine(pathToNewFolder, fileName);
                if (!File.Exists(pathToNewFile))
                {
                    webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
                }                  

            }
            catch (Exception ex)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
        }
              
    }
}
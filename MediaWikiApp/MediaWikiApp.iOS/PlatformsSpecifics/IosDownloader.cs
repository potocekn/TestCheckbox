using AppBase.Interfaces;
using AppBase.iOS;
using Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IosDownloader))]
namespace AppBase.iOS
{
    /// <summary>
    /// Custom dependency service for downloading files on iOS platform.
    /// </summary>
    public class IosDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        /// <summary>
        /// Method used for downloading files.
        /// </summary>
        /// <param name="url">url of the file</param>
        /// <param name="folder">folder into which to store the file</param>
        /// <param name="fileName">name under which to store the file</param>
        public void DownloadFile(string url, string folder, string fileName)
        {
            string pathToNewFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), folder);
            Directory.CreateDirectory(pathToNewFolder);

            try
            {
                WebClient webClient = new WebClient();
                string pathToNewFile = Path.Combine(pathToNewFolder, fileName);
                webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
            }
            catch (Exception ex)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
        }      
    }
}
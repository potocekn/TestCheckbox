using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.Interfaces
{
    /// <summary>
    /// Interface providing all necessary methods for downloading files.
    /// </summary>
    public interface IDownloader
    {
        void DownloadFile(string url, string folder, string fileName);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
    }

    /// <summary>
    /// Class derived from EventArgs that containg all necessary event arguments for custom downloader.
    /// </summary>
    public class DownloadEventArgs : EventArgs
    {
        public bool FileSaved = false;
        public DownloadEventArgs(bool fileSaved)
        {
            FileSaved = fileSaved;
        }
    }
}

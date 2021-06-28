using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.Models
{
    /// <summary>
    /// Class that represents files with their actual version number. This class is used when determining if new version 
    /// of resource is available and should be downloaded.
    /// </summary>
    class ChangesItem
    {
        public string FileName { get; set; }
        public int VersionNumber { get; set; }
    }
}

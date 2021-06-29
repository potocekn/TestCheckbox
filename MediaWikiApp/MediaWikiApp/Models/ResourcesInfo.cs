using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.Models
{
    /// <summary>
    /// Class representing necessary information about downloaded resources 
    /// such as language of resource, name, file name and full path to the file. 
    /// </summary>
    public class ResourcesInfo
    {
        public string Language { get; set; }
        public string ResourceName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Version { get; set; } = 0;
        public string Url { get; set; }
    }
}

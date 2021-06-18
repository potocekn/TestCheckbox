using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AppBase
{
    /// <summary>
    /// Interface for saving files to disk.
    /// </summary>
    public interface ILocalFileProvider
    {
        Task<string> SaveFileToDisk(Stream stream, string fileName);
    }
}

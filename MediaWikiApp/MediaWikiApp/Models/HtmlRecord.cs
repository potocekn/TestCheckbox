using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AppBase.Models
{
    /// <summary>
    /// Class representing one record in the database for HTML resources.
    /// Primary key is ID.
    /// </summary>
    public class HtmlRecord
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int VersionNumber { get; set; }
        public string PageName { get; set; }
        public string PageLanguage { get; set; }
        public string PageContent { get; set; }
    }
}

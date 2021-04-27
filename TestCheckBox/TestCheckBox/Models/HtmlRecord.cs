using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AppBase.Models
{
    public class HtmlRecord
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string PageName { get; set; }
        public string PageLanguage { get; set; }
        public string PageContent { get; set; }
    }
}

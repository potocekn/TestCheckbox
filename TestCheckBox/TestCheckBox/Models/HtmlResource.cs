using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.Models
{
    /// <summary>
    /// Class representing a HTML resource. This class is used for displaying the HTML resource in the app.
    /// PageName represents what should be displayed on the button. 
    /// Command OpenResource is used to open the WebView with the resource content.
    /// </summary>
    public class HtmlResource
    {
        public string PageName { get; set; }
        public Command OpenResource { get; set; }
    }
}

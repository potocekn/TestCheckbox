using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppBase.Controls
{
    /// <summary>
    /// Class used for setting gradient of colors as a background of the application.
    /// </summary>
    public class GradientColorStack : StackLayout
    {
        /// <summary>
        /// Color at the top of the vertical gradient and on the left of the horizontal gradient.
        /// </summary>
        public Color StartColor
        {
            get;
            set;
        }

        /// <summary>
        /// Color at the bottom of the vertical gradient and on the right of the horizontal gradient.
        /// </summary>
        public Color EndColor
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBase.PopUpPages
{
    /// <summary>
    /// Class that represents custom popup that has a title, body, confirm and cancel buttons.
    /// This popup can currently be used only for Android 6.0 and higher. 
    /// It is planned to use this popup for iOS once bugs in the library will be fixed.
    /// </summary>    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YesNoPopUp : Popup
    {
        public YesNoPopUp(string message, string yes, string no, double width, double height)
        {
            InitializeComponent();
            yesNoPopupLabelBody.Text = message;
            yesPopupButton.Text = yes;
            noPopupButton.Text = no;
            this.Size = new Size(width, height);
        }

        /// <summary>
        /// Methos used when clicked on the cancel (NO) button.
        /// </summary>
        /// <param name="sender">The cancel button that was clicked on</param>
        /// <param name="e">Event arguments</param>
        private void noPopupButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(false);
        }

        /// <summary>
        /// Methos used when clicked on the confirm (YES) button.
        /// </summary>
        /// <param name="sender">The cancel button that was clicked on</param>
        /// <param name="e">Event arguments</param>
        private void yesPopupButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(true);
        }
    }
}
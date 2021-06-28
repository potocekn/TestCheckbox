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
    /// Class that represents custom popup that has a title, body and an OK button.
    /// This popup can currently be used only for Android 6.0 and higher. 
    /// It is planned to use this popup for iOS once bugs in the library will be fixed.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OKPopUp : Popup
    {
        public OKPopUp(string title, string message, string ok, double width, double height)
        {
            InitializeComponent();
            okPupupLabelTitle.Text = title;
            okPopupLabelBody.Text = message;
            okPopupButton.Text = ok;
            this.Size = new Size(width, height);
        }

        /// <summary>
        /// Method used when clicked on the OK button.
        /// </summary>
        /// <param name="sender">OK button that was clicked on</param>
        /// <param name="e">Event arguments</param>
        private void okPopupButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(true);
        }
    }
}
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

        private void noPopupButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(false);
        }

        private void yesPopupButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(true);
        }
    }
}
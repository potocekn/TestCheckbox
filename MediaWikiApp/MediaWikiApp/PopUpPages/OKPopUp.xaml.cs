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
    public partial class OKPopUp : Popup
    {
        public OKPopUp(string title, string message, string ok)
        {
            InitializeComponent();
            okPupupLabelTitle.Text = title;
            okPopupLabelBody.Text = message;
            okPopupButton.Text = ok;
        }

        private void okPopupButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(true);
        }
    }
}
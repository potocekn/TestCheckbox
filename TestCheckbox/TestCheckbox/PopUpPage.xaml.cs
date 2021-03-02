using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpPage 
    {
        public PopUpPage()
        {
            InitializeComponent();
        }

        private void ClickedYes(object o, EventArgs e)
        {
            yesButton.Text = "clicked";
        }
    }
}
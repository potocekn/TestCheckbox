﻿using System;
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
        public OKPopUp(string title, string message, string ok, double width, double height)
        {
            InitializeComponent();
            okPupupLabelTitle.Text = title;
            okPopupLabelBody.Text = message;
            okPopupButton.Text = ok;
            this.Size = new Size(width, height);
        }

        private void okPopupButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(true);
        }
    }
}
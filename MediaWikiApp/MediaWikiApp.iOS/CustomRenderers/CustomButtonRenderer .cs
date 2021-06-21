using AppBase.iOS.CustomRenderers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Button), typeof(CustomButtonRenderer))]
namespace AppBase.iOS.CustomRenderers
{   
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
                Control.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
        }
    }
}
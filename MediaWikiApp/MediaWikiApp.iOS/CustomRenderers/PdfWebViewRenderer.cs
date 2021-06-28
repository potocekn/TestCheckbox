using AppBase.Controls;
using AppBase.iOS.CustomRenderers;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PdfWebView), typeof(PdfWebViewRenderer))]
namespace AppBase.iOS.CustomRenderers
{
    /// <summary>
	/// Custom renderer for showing PDF files on iOS platform.
    /// </summary>
    public class PdfWebViewRenderer : WkWebViewRenderer
    {
        /// <summary>
        /// Method used when there are changes in the WebView element.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (NativeView != null && e.NewElement != null)
            {
                var pdfControl = NativeView as UIWebView;

                if (pdfControl == null)
                    return;

                pdfControl.ScalesPageToFit = true;
            }
        }
    }
}
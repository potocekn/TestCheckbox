using System;
using AppBase.Controls;
using TestCheckbox.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(GradientColorStack), typeof(GradientColorStackRenderer))]
namespace TestCheckbox.Droid
{
#pragma warning disable CS0618 // Type or member is obsolete
    /// <summary>
    /// Custom renderer used for creating background as a gradiend of 2 colours on Android platform.
    /// </summary>
    public class GradientColorStackRenderer : VisualElementRenderer<StackLayout>
{
        private Color StartColor
        {
            get;
            set;
        }
        private Color EndColor
        {
            get;
            set;
        }

        /// <summary>
        /// Method for drawing the gradient.
        /// </summary>
        /// <param name="canvas">canvas where to draw</param>
        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            var gradient = new Android.Graphics.LinearGradient(0, 0, 0, Height, this.StartColor.ToAndroid(), this.EndColor.ToAndroid(),
            Android.Graphics.Shader.TileMode.Mirror);
            var paint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawPaint(paint);
            base.DispatchDraw(canvas);
        }

        /// <summary>
        /// Method used when a change in a Stack layout occurs.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
            {
                return;
            }
            try
            {
                var stack = e.NewElement as GradientColorStack;
                this.StartColor = stack.StartColor;
                this.EndColor = stack.EndColor;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}  
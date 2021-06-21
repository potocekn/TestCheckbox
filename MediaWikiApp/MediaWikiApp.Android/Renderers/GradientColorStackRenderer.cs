using System;
using AppBase.Controls;
using TestCheckbox.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(GradientColorStack), typeof(GradientColorStackRenderer))]
namespace TestCheckbox.Droid
{
#pragma warning disable CS0618 // Type or member is obsolete
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
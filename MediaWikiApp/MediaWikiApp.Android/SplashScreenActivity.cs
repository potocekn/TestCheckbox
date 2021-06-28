using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidX.AppCompat.App;
using AppBaseNamespace.Droid;

namespace TestCheckbox.Droid
{
    /// <summary>
    /// Custom activity used to display a splash screen.
    /// </summary>
    [Activity(Theme = "@style/SplashScreen", MainLauncher = true, NoHistory = true)]
    class SplashScreenActivity: AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState) 
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                var intent = new Intent(this, typeof(MainActivity));
                if (Intent.Extras != null)
                {
                    intent.PutExtras(Intent.Extras);
                }

                StartActivity(intent);
            }
            catch
            { 
            }
        }
    }
}
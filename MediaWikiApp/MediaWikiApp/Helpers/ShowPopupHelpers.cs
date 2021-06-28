using AppBase.PopUpPages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace AppBase.Helpers
{
    public static class ShowPopupHelpers
    {
        public static async Task ShowOKPopup(ContentPage page, string title, string message, double width, double heightAndroid)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    await page.DisplayAlert(title, message, "OK");
                    break;
                case Device.Android:
                default:
                    Version minVersion = new Version("6.0");
                    if (Xamarin.Essentials.DeviceInfo.Version < minVersion)
                    {
                        await page.DisplayAlert(title, message, "OK");
                    }
                    else
                    {
                        await page.Navigation.ShowPopupAsync(new OKPopUp(title, message, "OK", width, heightAndroid));
                    }
                    break;
            }
        }

        public static async Task<bool> ShowYesNoPopup(ContentPage page, string message, string yes, string no, double width, double height)
        {
            bool result = false;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    result = await page.DisplayAlert("", message, yes, no);
                    break;
                case Device.Android:
                default:
                    Version minVersion = new Version("6.0");
                    if (Xamarin.Essentials.DeviceInfo.Version < minVersion)
                    {
                       result = await page.DisplayAlert("", message, yes, no);
                    }
                    else
                    {
                        result = (bool)await page.Navigation.ShowPopupAsync(new YesNoPopUp(message, yes, no, width, height));
                    }
                    break;
            }
            return result;
        }
    }
}

using AppBase.PopUpPages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace AppBase.Helpers
{
    /// <summary>
    /// Class that contains methods for showing popup messages. 
    /// Custom popups available on Android devices from the version 6.0 and higher.
    /// The lower versions of Android and iOS have the default popups. 
    /// For iOS there is a goal to create custom popups in the future when the bugs in the library will be resolved.
    /// </summary>
    public static class ShowPopupHelpers
    {
        /// <summary>
        /// A method for showing popup that has only OK button, title and message. 
        /// This popups are used for informing user about current status. 
        /// For example start of the downloading, no internet connection, ...
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="title">Title of the popup</param>
        /// <param name="message">Text displayed in the body of the popup</param>
        /// <param name="width">Width of the popup in pixels</param>
        /// <param name="heightAndroid">Height of the popup on the Android platform in pixels</param>
        /// <returns></returns>
        public static async Task ShowOKPopup(ContentPage page, string title, string message, double width, double heightAndroid)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    await page.DisplayAlert(title, message, Constants.OK);
                    break;
                case Device.Android:
                default:
                    Version minVersion = new Version(Constants.MIN_ANDROID_VERSION_FOR_CUSTOM_POPUP);
                    if (Xamarin.Essentials.DeviceInfo.Version < minVersion)
                    {
                        await page.DisplayAlert(title, message, Constants.OK);
                    }
                    else
                    {
                        await page.Navigation.ShowPopupAsync(new OKPopUp(title, message, Constants.OK, width, heightAndroid));
                    }
                    break;
            }
        }

        /// <summary>
        /// A method for displaying a popup that expects user to confirm or cancel the performance of the action.
        /// This popup is used for change of the application language. 
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="message">Message displayed in the body of the popup</param>
        /// <param name="yes">Message for the confirm button</param>
        /// <param name="no">Message for the cancel button</param>
        /// <param name="width">Width of the popup in pixels</param>
        /// <param name="height">Height of the popup in pixels</param>
        /// <returns></returns>
        public static async Task<bool> ShowYesNoPopup(ContentPage page, string message, string yes, string no, double width, double height)
        {
            bool result = false;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    result = await page.DisplayAlert(Constants.EMPTY_STRING, message, yes, no);
                    break;
                case Device.Android:
                default:
                    Version minVersion = new Version(Constants.MIN_ANDROID_VERSION_FOR_CUSTOM_POPUP);
                    if (Xamarin.Essentials.DeviceInfo.Version < minVersion)
                    {
                       result = await page.DisplayAlert(Constants.EMPTY_STRING, message, yes, no);
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

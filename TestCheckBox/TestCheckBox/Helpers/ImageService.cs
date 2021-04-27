using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppBase.Helpers
{
    /// <summary>
    /// Class containing important functions for working with images in the app.
    /// </summary>
    public static class ImageService
    {
        static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// Static method that downloads image from given URL address and returns it as byte array.
        /// </summary>
        /// <param name="imageUrl">URL of the image that should be downloaded</param>
        /// <returns>downloaded image in a form of byte array</returns>
        public static Task<byte[]> DownloadImage(string imageUrl)
        {
            if (!imageUrl.Trim().StartsWith("https", StringComparison.OrdinalIgnoreCase))
                throw new Exception("iOS and Android Require Https");

            return _client.GetByteArrayAsync(imageUrl);
        }

        /// <summary>
        /// Static method used for saving specified image to the device disk.
        /// </summary>
        /// <param name="imageFileName">under what name should the image be saved</param>
        /// <param name="imageAsBase64String">content of the image file in a form of byte array</param>
        public static void SaveToDisk(string imageFileName, byte[] imageAsBase64String)
        {
            Xamarin.Essentials.Preferences.Set(imageFileName, Convert.ToBase64String(imageAsBase64String));
        }

        /// <summary>
        /// Static method used for retrieving image from disk.
        /// </summary>
        /// <param name="imageFileName">name of the image that should be retrieved</param>
        /// <returns>ImageSource of specified image file</returns>
        public static Xamarin.Forms.ImageSource GetFromDisk(string imageFileName)
        {
            var imageAsBase64String = Xamarin.Essentials.Preferences.Get(imageFileName, string.Empty);

            return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imageAsBase64String)));
        }
    }
}

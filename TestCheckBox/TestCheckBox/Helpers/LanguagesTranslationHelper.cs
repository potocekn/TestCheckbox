using AppBase.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.Helpers
{
    public static class LanguagesTranslationHelper
    {
        public static string ReturnTranslation(string original)
        {
            switch (original)
            {
                case "English":
                    return AppResources.englishLabel_Text;
                case "Czech":
                    return AppResources.czechLabel_Text;
                case "German":
                    return AppResources.germanLabel_Text;
                default:
                    return "";
            }
        }
    }
}

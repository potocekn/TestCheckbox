using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.Helpers
{
    /// <summary>
    /// Class that contains the constants that are used in several places in the program. 
    /// These constants are stored here to lower thr risk of a mistake (misspelling, ...)
    /// and to make changing the value easier (only one place to change).
    /// </summary>
    public static class Constants
    {
        public const string ENGLISH_LANGUAGE_NAME = "English";
        public const string GERMAN_LANGUAGE_NAME = "German";
        public const string CZECH_LANGUAGE_NAME = "Czech";
        public const string FRENCH_LANGUAGE_NAME = "French";
        public const string CHINESE_LANGUAGE_NAME = "Chinese";

        public const string ENGLISH_LANGUAGE_SHORTCUT = "en";
        public const string GERMAN_LANGUAGE_SHORTCUT = "de";
        public const string CZECH_LANGUAGE_SHORTCUT = "cs";
        public const string FRENCH_LANGUAGE_SHORTCUT = "fr";
        public const string CHINESE_LANGUAGE_SHORTCUT = "zh-Hans";

        public const string NAVIGATION_PAGE_BACKGROUND_COLOR = "#B3BAE4";

        public const string OK = "OK";

        public const string HTML = "HTML";
        public const string PDF = "PDF";
        public const string ODT = "ODT";

        public const string DASH = "-";
        public const string FORWARD_SLASH = "/";
        public const string EMPTY_STRING = "";
        public const string NEW_LINE = "\n";

        public const string MIN_ANDROID_VERSION_FOR_CUSTOM_POPUP = "6.0";

        public const string AUTOMATIC = "Automatic";
        public const string ONCE_A_MONTH = "Once a Month";
        public const string ON_REQUEST = "On Request";

        public const string WIFI_TOGGLE_NAME = "wifi";

        public const string IS_CHECKED_PROPERTY_NAME = "IsChecked";
    }
}

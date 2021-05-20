using AppBase.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.Helpers
{
    /// <summary>
    /// Class containing helper methods for Popup messages.
    /// </summary>
    public static class PopupMessageHelpers
    {
        /// <summary>
        /// Method that creates message related to the change of app language.
        /// </summary>
        /// <param name="from">Language in which the message should be created.</param>
        /// <returns>Created message in given language in string form.</returns>
        public static string CreateMessageFrom(string from)
        {
            StringBuilder result = new StringBuilder();
            switch (from)
            {
                case "English":
                    result.Append(AppResources.LanguageChangeQuestionInEnglish_Text + '\n');
                    result.Append(AppResources.ActionWillRestartApplicationInEnglish_Text + '\n');
                    break;
                case "German":
                    result.Append(AppResources.LanguageChangeQuestionInGerman_Text + '\n');
                    result.Append(AppResources.ActionWillRestartApplicationInGerman_Text + '\n');
                    break;
                case "Czech":
                    result.Append(AppResources.LanguageChangeQuestionInCzech_Text + '\n');
                    result.Append(AppResources.ActionWillRestartApplicationInCzech_Text + '\n');
                    break;
                case "French":
                    result.Append(AppResources.LanguageChangeQuestionInFrench_Text + '\n');
                    result.Append(AppResources.ActionWillRestartApplicationInFrench_Text + '\n');
                    break;
                case "Chinese":
                    result.Append(AppResources.LanguageChangeQuestionInChinese_Text + '\n');
                    result.Append(AppResources.ActionWillRestartApplicationInChinese_Text + '\n');
                    break;
                default:
                    result.Append(AppResources.LanguageChangeQuestionInEnglish_Text + '\n');
                    result.Append(AppResources.ActionWillRestartApplicationInEnglish_Text + '\n');
                    break;
            }

            return result.ToString();
        }

        /// <summary>
        /// Method that creates message related to the application language change in 2 given languages.
        /// In case of both languages being the same only one part of the message is created.
        /// </summary>
        /// <param name="from">Name of the language that represents current application language.</param>
        /// <param name="to">Name of the language that represents desired application language. </param>
        /// <returns></returns>
        public static string CreatePopUpMessage(string from, string to)
        {
            StringBuilder result = new StringBuilder();
            if (from != to)
            {
                result.Append(CreateMessageFrom(from));
                result.Append("\n");
                result.Append(CreateMessageFrom(to));
            }
            else
            {
                result.Append(CreateMessageFrom(from));
            }
            return result.ToString();
        }

        /// <summary>
        /// Method that returns translation of the word "yes" in given language.
        /// Default is "yes".
        /// </summary>
        /// <param name="from">language for the translation</param>
        /// <returns>translation of the word "yes" in string form</returns>
        public static string CreateYesMessageFrom(string from)
        {
            StringBuilder result = new StringBuilder();
            switch (from)
            {
                case "English":
                    result.Append(AppResources.YesInEnglish_Text);
                    break;
                case "German":
                    result.Append(AppResources.YesInGerman_Text);
                    break;
                case "Czech":
                    result.Append(AppResources.YesInCzech_Text);
                    break;
                case "French":
                    result.Append(AppResources.YesInFrench_Text);
                    break;
                case "Chinese":
                    result.Append(AppResources.YesInChinese_Text);
                    break;
                default:
                    result.Append(AppResources.YesInEnglish_Text);
                    break;
            }

            return result.ToString();
        }

        /// <summary>
        /// Creates multilingual message that says "yes" in two languages. This is used as a part of popup message for app language change.
        /// </summary>
        /// <param name="from">current language of the application</param>
        /// <param name="to">language to which app will change</param>
        /// <returns>created message in given 2 languages</returns>
        public static string CreateYesMessage(string from, string to)
        {
            StringBuilder result = new StringBuilder();
            if (from != to)
            {
                result.Append(CreateYesMessageFrom(from));
                result.Append("/");
                result.Append(CreateYesMessageFrom(to));
            }
            else
            {
                result.Append(CreateYesMessageFrom(from));
            }
            return result.ToString();
        }

        /// <summary>
        /// Method that returns translation of the word "no" in given language.
        /// Default is "no".
        /// </summary>
        /// <param name="from">language for the translation</param>
        /// <returns>translation of the word "no" in string form</returns>
        public static string CreateNoMessageFrom(string from)
        {
            StringBuilder result = new StringBuilder();
            switch (from)
            {
                case "English":
                    result.Append(AppResources.NoInEnglish_Text);
                    break;
                case "German":
                    result.Append(AppResources.NoInGerman_Text);
                    break;
                case "Czech":
                    result.Append(AppResources.NoInCzech_Text);
                    break;
                case "French":
                    result.Append(AppResources.NoInFrench_Text);
                    break;
                case "Chinese":
                    result.Append(AppResources.NoInChinese_Text);
                    break;
                default:
                    result.Append(AppResources.NoInEnglish_Text);
                    break;
            }

            return result.ToString();
        }

        /// <summary>
        /// Creates multilingual message that says "no" in two languages. This is used as a part of popup message for app language change.
        /// </summary>
        /// <param name="from">current language of the application</param>
        /// <param name="to">language to which app will change</param>
        /// <returns>created message in given 2 languages</returns>
        public static string CreateNoMessage(string from, string to)
        {
            StringBuilder result = new StringBuilder();
            if (from != to)
            {
                result.Append(CreateNoMessageFrom(from));
                result.Append("/");
                result.Append(CreateNoMessageFrom(to));
            }
            else
            {
                result.Append(CreateNoMessageFrom(from));
            }
            return result.ToString();
        }
    }
}

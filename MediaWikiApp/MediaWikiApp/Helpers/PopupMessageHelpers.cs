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
                    result.Append("Would you like to change language of the application?\n");
                    result.Append("This action will restart the application!\n");
                    break;
                case "German":
                    result.Append("Möchten Sie die Sprache der Anwendung ändern?\n");
                    result.Append("Diese Aktion startet die Anwendung neu!\n");
                    break;
                case "Czech":
                    result.Append("Chcete změnit jazyk aplikace?\n");
                    result.Append("Tato akce restartuje aplikaci!\n");
                    break;
                case "French":
                    result.Append("Souhaitez-vous changer la langue de l'application?\n");
                    result.Append("Cette action redémarrera l'application!\n");
                    break;
                case "Chinese":
                    result.Append("您想更改應用程序的語言嗎?\n");
                    result.Append("此操作將重新啟動應用程序!\n");
                    break;
                default:
                    result.Append("Would you like to change language of the application?\n");
                    result.Append("This action will restart the application!\n");
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
                    result.Append("Yes");
                    break;
                case "German":
                    result.Append("Ja");
                    break;
                case "Czech":
                    result.Append("Ano");
                    break;
                case "French":
                    result.Append("Oui");
                    break;
                case "Chinese":
                    result.Append("是的");
                    break;
                default:
                    result.Append("Yes");
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
                    result.Append("No");
                    break;
                case "German":
                    result.Append("Nein");
                    break;
                case "Czech":
                    result.Append("Ne");
                    break;
                case "French":
                    result.Append("Non");
                    break;
                case "Chinese":
                    result.Append("是的");
                    break;
                default:
                    result.Append("不");
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

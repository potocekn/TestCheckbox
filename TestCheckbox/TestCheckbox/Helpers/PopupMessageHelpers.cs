using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.Helpers
{
    public static class PopupMessageHelpers
    {
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

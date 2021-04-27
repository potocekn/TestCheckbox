using AppBase.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.Helpers
{
    /// <summary>
    /// Class containing helper methods for work with multilingual app tool kit resources.
    /// </summary>
    public static class LanguagesTranslationHelper
    {
        /// <summary>
        /// Static method that returns value of label for given language. This method is needed for rendering language names in multilingual mode.
        /// By returning the value of label it returns already translated string representation of language name (multilingual app tool kit translated it).
        /// </summary>
        /// <param name="original">English version of language name starting with capital letter (e.g.: English or German)</param>
        /// <returns>string representation of language name took from multilingual resources based on current language of application</returns>
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
                case "French":
                    return AppResources.frenchLabel_Text;
                case "Chinese":
                    return AppResources.chineseLabel_Text;
                case "Arabic":
                    return AppResources.arabicLabel_Text;
                case "Azerbaijani":
                    return AppResources.azerbaijaniLabel_Text;
                case "Bulgarian":
                    return AppResources.bulgarianLabel_Text;
                case "Spanish":
                    return AppResources.spanishLabel_Text;
                case "Estonian":
                    return AppResources.estonianLabel_Text;
                case "Persian":
                    return AppResources.persianLabel_Text;
                case "Hausa":
                    return AppResources.hausaLabel_Text;
                case "Hindi":
                    return AppResources.hindiLabel_Text;
                case "Croatian":
                    return AppResources.croatianLabel_Text;
                case "Hungarian":
                    return AppResources.hungarianLabel_Text;
                case "Indonesian":
                    return AppResources.indonesianLabel_Text;
                case "Italian":
                    return AppResources.italianLabel_Text;
                case "Georgian":
                    return AppResources.georgianLabel_Text;
                case "Kannada":
                    return AppResources.kannadaLabel_Text;
                case "Korean":
                    return AppResources.koreanLabel_Text;
                case "Central Kurdish":
                    return AppResources.centralKurdishLabel_Text;
                case "Kyrgyz":
                    return AppResources.kyrgyzLabel_Text;
                case "Malayalam":
                    return AppResources.malayalamLabel_Text;
                case "Malay":
                    return AppResources.malayLabel_Text;
                case "Norwegian (Bokmål)":
                    return AppResources.norwegianBokmalLabel_Text;
                case "Dutch":
                    return AppResources.dutchLabel_Text;
                case "Polish":
                    return AppResources.polishLabel_Text;
                case "Pashto":
                    return AppResources.pashtoLabel_Text;
                case "Portuguese":
                    return AppResources.portugueseLabel_Text;
                case "Portuguese (Brazil)":
                    return AppResources.portugueseBrazilLabel_Text;
                case "Romanian":
                    return AppResources.romanianLabel_Text;
                case "Russian":
                    return AppResources.russianLabel_Text;
                case "Slovak":
                    return AppResources.slovakLabel_Text;
                case "Slovenian":
                    return AppResources.slovenianLabel_Text;
                case "Albanian":
                    return AppResources.albanianLabel_Text;
                case "Serbian":
                    return AppResources.serbianLabel_Text;
                case "Swedish":
                    return AppResources.swedishLabel_Text;
                case "Kiswahili":
                    return AppResources.kiswahiliLabel_Text;
                case "Tamil":
                    return AppResources.tamilLabel_Text;
                case "Telugu":
                    return AppResources.teluguLabel_Text;
                case "Thai":
                    return AppResources.thaiLabel_Text;
                case "Tigrinya":
                    return AppResources.tigrinyaLabel_Text;
                case "Turkish":
                    return AppResources.turkishLabel_Text;
                case "Ukrainian":
                    return AppResources.ukrainianLabel_Text;
                case "Urdu":
                    return AppResources.urduLabel_Text;
                case "Uzbek":
                    return AppResources.uzbekLabel_Text;
                case "Uzbek (Cyrillic)":
                    return AppResources.uzbekCyrillicLabel_Text;
                case "Vietnamese":
                    return AppResources.vietnameseLabel_Text;
                case "isiXhosa":
                    return AppResources.isiXhosaLabel_Text;
                default:
                    return "";
            }
        }
    }
}

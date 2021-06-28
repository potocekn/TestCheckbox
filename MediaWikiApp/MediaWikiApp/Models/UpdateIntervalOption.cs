using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.Models
{
    /// <summary>
    /// Enum representing the type of the interval update option.
    /// </summary>
    public enum UpdateIntervalOption
    {
        AUTOMATIC,
        ONCE_A_MONTH,
        ON_REQUEST
    }

    /// <summary>
    /// Methods for working with the UpdateIntervalOption enum options.
    /// </summary>
    public static class UpdateintervalOptionExtensions
    {
        /// <summary>
        /// Method for translating string to enum type.
        /// </summary>
        /// <param name="senderEnglishName">string to convert</param>
        /// <returns></returns>
        public static UpdateIntervalOption GetUpdateIntervalOption(string senderEnglishName)
        {
            switch (senderEnglishName)
            {
                case "Automatic":
                    return UpdateIntervalOption.AUTOMATIC;
                case "Once a Month":
                    return UpdateIntervalOption.ONCE_A_MONTH;
                case "On Request":
                    return UpdateIntervalOption.ON_REQUEST;
                default:
                    return UpdateIntervalOption.AUTOMATIC;
            }
        }
    }
}

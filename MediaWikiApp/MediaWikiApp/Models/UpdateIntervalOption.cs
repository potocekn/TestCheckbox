using System;
using System.Collections.Generic;
using System.Text;

namespace AppBase.Models
{
    public enum UpdateIntervalOption
    {
        AUTOMATIC,
        ONCE_A_MONTH,
        ON_REQUEST
    }

    public static class UpdateintervalOptionExtensions
    {
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

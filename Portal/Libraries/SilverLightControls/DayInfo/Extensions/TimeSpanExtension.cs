using System;
using System.Text;
using DayInfo.Resources;

namespace DayInfo.Extensions
{
    public static class TimeSpanExtension
    {
        public static string NumericTxt(int Number,
        string nominative, //Именительный падеж
        string genitive_singular,//Родительный падеж, единственное число
        string genitive_plural) //Родительный падеж, множественное число
        {
            int[] FormsTable = { 2, 0, 1, 1, 1, 2, 2, 2, 2, 2 };

            Number = Math.Abs(Number);
            int res = FormsTable[((((Number % 100) / 10) != 1) ? 1 : 0) * (Number % 10)];
            switch (res)
            {
                case 0:
                    return nominative;
                case 1:
                    return genitive_singular;
                default:
                    return genitive_plural;
            }
        }

        public static string TimeToString(this TimeSpan targetTime)
        {
            StringBuilder result = new StringBuilder();
            Resource Res = new Resource();

            int hours = (int)targetTime.TotalHours;
            int minutes = (int)(targetTime - new TimeSpan(hours, 0, 0)).TotalMinutes;

            if (hours != 0)
                result.AppendFormat(
                    NumericTxt(hours, Res.msg_HoursI, Res.msg_HoursR1, Res.msg_HoursRm),
                    hours);

            if (minutes != 0)
            {
                if (result.Length > 0)
                    result.Append(", ");

                result.AppendFormat(
                    NumericTxt(minutes, Res.msg_MinutesI, Res.msg_MinutesR1, Res.msg_MinutesRm), minutes);
            }

            return result.ToString();
        }
    }
}

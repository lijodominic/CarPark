using System;
using System.Linq;

namespace CarParkRateCommon
{
    public static class Helper
    {
        public static Tuple<DateTime?, DateTime?> GetDateTimeByCondition(string condition)
        {
            var splitByTo = condition.Split("To").Select(p => p.Trim()).ToList();
            if (splitByTo.Count == 2)
            {
                DateTime? startduration = GetValidDateTime(splitByTo[0], true);
                DateTime? endDuration = GetValidDateTime(splitByTo[1], true);
                if (splitByTo[1] == "00:00")
                {
                    endDuration = endDuration.Value.AddDays(1);
                }
                if (startduration.HasValue && endDuration.HasValue)
                {
                    if (endDuration > startduration)
                    {
                        return new Tuple<DateTime?, DateTime?>(startduration, endDuration);
                    }
                }
            }
            return null;
        }

        public static DateTime? ConvertStringToTimeSpan(string condition)
        {
            return GetValidDateTime(condition, true);
        }

        public static bool IsWeekDay(this DateTime dateTime)
        {
            return dateTime.DayOfWeek >= DayOfWeek.Monday && dateTime.DayOfWeek <= DayOfWeek.Friday;
        }

        public static bool IsWeekEnd(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool IsValidDateTime(this string strDateTime)
        {
            return DateTime.TryParse(strDateTime, out DateTime dateTime);
        }

        public static DateTime? GetValidDateTime(this string strDateTime, bool isDateTimeValidate)
        {
            if (!isDateTimeValidate || strDateTime.IsValidDateTime())
            {
                DateTime.TryParse(strDateTime, out DateTime dateTime);
                return dateTime;
            }

            return null;
        }


    }
}

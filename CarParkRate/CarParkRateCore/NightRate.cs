using CarParkRateCommon;
using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CarParkRateCore
{
    public class NightRate : ParkingRate
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _name;
        private readonly RateType _type;
        const double Rate = 6.50;
        const string EntryCondition = "18:00 To 00:00";
        const string ExitCondition = "08:00";
        public NightRate()
        {
            _name = "Night Rate";
            _type = RateType.FlatRate;
        }

        public override string Name { get => _name; }
        public override RateType Type { get => _type; }

        public override double? CalculateCost(DateTime entry, DateTime exit)
        {
            var entryStartTimeCondition = Helper.GetDateTimeByCondition(EntryCondition);
            var exitEndTimeCondition = Helper.ConvertStringToTimeSpan(ExitCondition);
            if (entryStartTimeCondition != null && exitEndTimeCondition != null)
            {
                var entryStartTimeConditionValue = entryStartTimeCondition.Item1.Value.TimeOfDay;
                var entryEndTimeCondition = entryStartTimeCondition.Item2.Value.TimeOfDay;
                var exitEndTimeConditionValue = exitEndTimeCondition.Value.TimeOfDay;
                var entryEndTime = new DateTime(entry.Year, entry.Month, entry.Day).Add(entryEndTimeCondition).AddDays(1);
                var exitEndTime = new DateTime(exit.Year, exit.Month, exit.Day).Add(exitEndTimeConditionValue).AddDays(1);

                if ((entry.TimeOfDay >= entryStartTimeConditionValue && entry <= entryEndTime) &&
                    (exit <= exitEndTime) && (exit.Subtract(entry).TotalDays <= 1) && entry.IsWeekDay())
                {
                    return Rate;
                }
                else
                {
                    Log.Info("Condition not met to apply NightRate");
                }
            }
            else
            {
                Log.Error("Condition Set is wrong, please validate.Was not able to verify this rate for the customer");
            }

            return null;
        }
    }
}

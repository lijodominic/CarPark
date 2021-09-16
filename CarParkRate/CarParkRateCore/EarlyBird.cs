using CarParkRateCommon;
using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace CarParkRateCore
{
    public class EarlyBird : ParkingRate
    {
        private readonly string _name;
        private readonly RateType _type;
        const double Rate = 13;
        const string EntryCondition = "6:00 To 9:00";
        const string ExitCondition = "15:30 To 23:30";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public EarlyBird()
        {
            _name = "Early Bird";
            _type = RateType.FlatRate;
        }

        public override string Name { get => _name; }
        public override RateType Type { get => _type; }

        public override double? CalculateCost(DateTime entry, DateTime exit)
        {
            var entryStartTimeCondition = Helper.GetDateTimeByCondition(EntryCondition);
            var exitEndTimeCondition = Helper.GetDateTimeByCondition(ExitCondition);
            if (entryStartTimeCondition != null && exitEndTimeCondition != null)
            {
                var entryStartTimeConditionValue = entryStartTimeCondition.Item1.Value.TimeOfDay;
                var entryEndTimeCondition = entryStartTimeCondition.Item2.Value.TimeOfDay;
                var exitStartTimeConditionValue = exitEndTimeCondition.Item1.Value.TimeOfDay;
                var exitEndTimeConditionvValue= exitEndTimeCondition.Item2.Value.TimeOfDay;
                if ((entry.TimeOfDay >= entryStartTimeConditionValue && entry.TimeOfDay <= entryEndTimeCondition) &&
                    (exit.TimeOfDay >= exitStartTimeConditionValue && exit.TimeOfDay <= exitEndTimeConditionvValue))
                {
                    return Rate;
                }
                else
                {
                    Log.Info("Condition not met to apply EarlyBirdRate");
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

using CarParkRateCommon;
using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace CarParkRateCore
{
    public class WeekEndRate : ParkingRate
    {
        private readonly string _name;
        private readonly RateType _type;
        const double Rate = 10;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public WeekEndRate()
        {
            _name = "Weekend Rate";
            _type = RateType.FlatRate;
        }

        public override string Name { get => _name; }
        public override RateType Type { get => _type; }

        public override double? CalculateCost(DateTime entry, DateTime exit)
        {
            if (entry.IsWeekEnd() && exit.IsWeekEnd())
            {
                return Rate;
            }
            else
            {
                Log.Info("Condition not met to apply WeekEndRate");
            }

            return null;
        }
    }
}

using CarParkRateCommon;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarParkRateCore
{

    public class StandardRate : ParkingRate
    {
        const double OneHourRate = 5;
        const double TwoHourRate = 10;
        const double ThreeHourRate = 15;
        const double ThreePlusHourRate = 20;
        private readonly string _name;
        private readonly RateType _type;
        //private readonly ILogger _logger;
        public StandardRate()
        {
            _name = "Standard Rate";
            _type = RateType.HourlyRate;
            //_logger = logger;
        }

        public override string Name { get => _name; }
        public override RateType Type { get => _type; }

        public override double? CalculateCost(DateTime entry, DateTime exit)
        {
            var hourDifference = exit.Subtract(entry).TotalHours;
            if (hourDifference <= 1)
            {
                return OneHourRate;
            }
            else if (hourDifference >= 1 && hourDifference <= 2)
            {
                return TwoHourRate;
            }
            else if (hourDifference >= 2 && hourDifference <= 3)
            {
                return ThreeHourRate;
            }
            else
            {
                return ThreePlusHourRate;
            }
        }
    }
}

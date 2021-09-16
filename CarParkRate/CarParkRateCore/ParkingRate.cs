using CarParkRateCommon;
using System;

namespace CarParkRateCore
{
    public abstract class ParkingRate
    {
        public abstract string Name { get; }

        public abstract RateType Type { get; }

        public abstract double? CalculateCost(DateTime entry, DateTime exit);
    }
}

using CarParkRateCommon;
using System;

namespace CarParkRateInterface
{
    public interface ICarParkRateService
    {
        public CarParkRateDTO GetParkingRate(string entryDateTime, string exitDateTime);
    }
}

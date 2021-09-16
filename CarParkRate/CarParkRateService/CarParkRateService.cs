using CarParkRateCommon;
using CarParkRateCore;
using CarParkRateInterface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

namespace CarParkRateService
{
    public class CarParkService : ICarParkRateService
    {
        private readonly ILogger _logger;
        public CarParkService(ILogger<CarParkService> logger)
        {
            _logger = logger;
        }
        public CarParkRateDTO GetParkingRate(string strEntryDateTime, string strExitDateTime)
        {
            if (!strEntryDateTime.IsValidDateTime())
            {
                _logger.LogError($"{strEntryDateTime} is not a valid date time.");
                throw new InvalidDate();
            }
            if (!strExitDateTime.IsValidDateTime())
            {
                _logger.LogError($"{strExitDateTime} is not a valid date time.");
                throw new InvalidDate();
            }
            DateTime? entryDateTime = strEntryDateTime.GetValidDateTime(false);
            DateTime? exitDateTime = strExitDateTime.GetValidDateTime(false);

            if (entryDateTime > exitDateTime)
            {
                _logger.LogError($"{strExitDateTime} is not a valid date time.");
                throw new InvalidDate();
            }

            System.Type[] possibleRates = Util.GetAllEntities();

            ConcurrentBag<CarParkRateDTO> carParkRates = new ConcurrentBag<CarParkRateDTO>();
            Parallel.ForEach(possibleRates, rate =>
            {
                ParkingRate parkingRate = (ParkingRate)Activator.CreateInstance(rate);
                carParkRates.Add(new CarParkRateDTO { Name = parkingRate.Name, TotalCost = parkingRate.CalculateCost(entryDateTime.Value, exitDateTime.Value) });
            });

            var validParkingRates = carParkRates.ToList().Where(c => c.TotalCost.HasValue);

            if (validParkingRates.Any())
            {
                return validParkingRates.OrderBy(c => c.TotalCost).First();
            }

            return new CarParkRateDTO();
        }
    }
}

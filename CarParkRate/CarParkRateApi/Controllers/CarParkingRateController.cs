using CarParkRateApi.Model;
using CarParkRateInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarParkRateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarParkingRateController : ControllerBase
    {
        private readonly ICarParkRateService _rateService;

        public CarParkingRateController(ICarParkRateService rateService)
        {
            _rateService = rateService;
        }

        [HttpPost]
        public string GetCost([FromBody] CarEntryExit carEntryExit)
        {
            if (string.IsNullOrEmpty(carEntryExit.EntryDate) && string.IsNullOrEmpty(carEntryExit.ExitDate))
            {
                return $"Not valid input found";
            }

            try
            {
                var rate = _rateService.GetParkingRate(carEntryExit.EntryDate, carEntryExit.ExitDate);

                return $"Name: {rate.Name}, Due: {rate.TotalCost}";
            }
            catch (Exception ex)
            {
                return $"Not valid input found:{ex.Message}";
            }
        }
    }
}

using CarParkRateCommon;
using CarParkRateInterface;
using CarParkRateService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarParkRateTests
{
    [TestClass]
    public class CarParkTests
    {
        private readonly ICarParkRateService _carParkService;
        public CarParkTests()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
           .AddSingleton<ICarParkRateService, CarParkService>().BuildServiceProvider();

            _carParkService = serviceProvider.GetService<ICarParkRateService>();
        }

        [DataRow("123324", "34545354543")]
        [DataRow("asdadas", "asdasds")]
        [DataTestMethod]
        public void ValidateInValidInput(string strEntry, string strExit)
        {
            Assert.ThrowsException<InvalidDate>(() => _carParkService.GetParkingRate(strEntry, strExit));
        }

        [DataRow("Friday, 29 May 2015")]
        [DataRow("Friday, 29 May 2015 05:50")]
        [DataRow("Friday, 29 May 2015 05:50 AM")]
        [DataRow("Friday, 29 May 2015 05:50:06")]
        [DataRow("29/05/2015 05:50")]
        [DataRow("29/05/2015 05:50 AM")]
        [DataRow("29/5/2015 05:50:06")]
        [DataRow("May 29")]
        [DataRow("2015-05-16T05:50:06.7199222-04:00")]
        [DataRow("2015-05-16T05:50:06")]
        [DataRow("05:50")]
        [DataRow("05:50:06")]
        [DataRow("05:50 AM")]
        [DataRow("16/09/2021 09:50 AM")]
        [DataTestMethod]
        public void ValidateValidInput(string strEntry)
        {
            Assert.IsTrue(Helper.IsValidDateTime(strEntry));
        }


        [DataRow("16/09/2021 09:50 AM", "16/09/2021 13:50 PM", "Standard Rate", 20)]
        [DataRow("16/09/2021 09:50 AM", "16/09/2021 10:40 AM", "Standard Rate", 5)]
        [DataRow("16/09/2021 09:50 AM", "16/09/2021 11:40 AM", "Standard Rate", 10)]
        [DataRow("16/09/2021 09:50 AM", "16/09/2021 12:40 PM", "Standard Rate", 15)]
        [DataTestMethod]
        public void GetStandardRate(string strEntry, string strExit, string name, double value)
        {
            var carfarePrice = _carParkService.GetParkingRate(strEntry, strExit);
            Assert.IsTrue(carfarePrice.Name == name);
            Assert.IsTrue(carfarePrice.TotalCost == value);
        }

        [DataRow("16/09/2021 07:50 AM", "16/09/2021 17:50 PM", "Early Bird", 13)]
        [DataRow("16/09/2021 08:50 AM", "16/09/2021 10:40 PM", "Early Bird", 13)]
        [DataRow("16/09/2021 06:50 AM", "16/09/2021 11:20 PM", "Early Bird", 13)]
        [DataRow("16/09/2021 08:50 AM", "16/09/2021 12:40 PM", "Standard Rate", 20)]
        [DataTestMethod]
        public void GetEarlyBirdRate(string strEntry, string strExit, string name, double value)
        {
            var carfarePrice = _carParkService.GetParkingRate(strEntry, strExit);
            Assert.IsTrue(carfarePrice.Name == name);
            Assert.IsTrue(carfarePrice.TotalCost == value);
        }


        [DataRow("16/09/2021 07:50 PM", "16/09/2021 11:50 PM", "Night Rate", 6.50)]
        [DataRow("16/09/2021 07:50 PM", "17/09/2021 07:50 PM", "Night Rate", 6.50)]
        [DataRow("17/09/2021 07:50 PM", "18/09/2021 07:50 PM", "Night Rate", 6.50)]
        [DataRow("18/09/2021 07:50 PM", "19/09/2021 07:50 PM", "Weekend Rate", 10)]
        [DataRow("16/09/2021 08:50 AM", "16/09/2021 10:40 PM", "Early Bird", 13)]
        [DataRow("16/09/2021 06:50 AM", "16/09/2021 11:20 PM", "Early Bird", 13)]
        [DataRow("16/09/2021 08:50 AM", "16/09/2021 12:40 PM", "Standard Rate", 20)]
        [DataTestMethod]
        public void GetNightRate(string strEntry, string strExit, string name, double value)
        {
            var carfarePrice = _carParkService.GetParkingRate(strEntry, strExit);
            Assert.IsTrue(carfarePrice.Name == name);
            Assert.IsTrue(carfarePrice.TotalCost == value);
        }

        [DataRow("18/09/2021 00:50 AM", "19/09/2021 23:50 PM", "Weekend Rate", 10)]
        [DataRow("18/09/2021 03:50 PM", "18/09/2021 04:30 PM", "Standard Rate", 5)]
        [DataRow("18/09/2021 03:50 PM", "18/09/2021 05:30 PM", "Weekend Rate", 10)]
        [DataRow("17/09/2021 07:50 PM", "18/09/2021 07:50 PM", "Night Rate", 6.50)]
        [DataRow("18/09/2021 07:50 PM", "19/09/2021 07:50 PM", "Weekend Rate", 10)]
        [DataRow("16/09/2021 08:50 AM", "16/09/2021 10:40 PM", "Early Bird", 13)]
        [DataRow("16/09/2021 06:50 AM", "16/09/2021 11:20 PM", "Early Bird", 13)]
        [DataRow("16/09/2021 08:50 AM", "16/09/2021 12:40 PM", "Standard Rate", 20)]
        [DataTestMethod]
        public void GetWeekEndRate(string strEntry, string strExit, string name, double value)
        {
            var carfarePrice = _carParkService.GetParkingRate(strEntry, strExit);
            Assert.IsTrue(carfarePrice.Name == name);
            Assert.IsTrue(carfarePrice.TotalCost == value);
        }
    }
}

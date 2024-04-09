using CarRentalAPI.IntegrationTests.Helpers;
using CarRentalAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarRentalAPI.IntegrationTests.Controllers
{
    public class CarControllerTests
    {
        private const string CAR_CONTROLLER_TEST_FOLDER = "TestData/CarController/";

        [Theory]
        [JsonData<QueryModel>(CAR_CONTROLLER_TEST_FOLDER + "GetAllCarsTestData.json", "ValidInput")]
        public async Task GetAllCars_ForValidModel_ReturnsOk(QueryModel query)
        {

        }
        [Theory]
        [JsonData<QueryModel>(CAR_CONTROLLER_TEST_FOLDER + "GetAllCarsTestData.json", "InvalidInput")]
        public async Task GetAllCars_ForInvalidModel_ReturnsBadRequest(QueryModel query)
        {

        }



    }
}

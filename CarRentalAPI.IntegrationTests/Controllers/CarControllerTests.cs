using CarRentalAPI.Entities;
using CarRentalAPI.IntegrationTests.Helpers;
using CarRentalAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog.Config;
using Xunit;

namespace CarRentalAPI.IntegrationTests.Controllers
{
    public class CarControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string CAR_CONTROLLER_TEST_FOLDER = "TestData/CarController/";
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        public CarControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<CarRentalDbContext>));
                        services.Remove(dbContextOptions);
                        services.AddDbContext<CarRentalDbContext>(options => options.UseInMemoryDatabase("CarRentalDb"));
                    });
                });

            _client = _factory.CreateClient();
        }

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

        [Theory]
        [JsonData<int>(CAR_CONTROLLER_TEST_FOLDER + "GetCarTestData.json", "ValidInput")]
        public async Task GetCar_ForValidModel_ReturnsOk(int id)
        {
            var response = await _client.GetAsync("api/cars/" + id);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [JsonData<int>(CAR_CONTROLLER_TEST_FOLDER + "GetCarTestData.json", "InvalidInput")]
        public async Task GetCar_ForInvalidModel_ReturnsNotFound(int id)
        {
            var response = await _client.GetAsync("api/cars/" + id);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

    }
}

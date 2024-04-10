using CarRentalAPI.Entities;
using CarRentalAPI.IntegrationTests.Helpers;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
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
                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                        services.AddDbContext<CarRentalDbContext>(options => options.UseInMemoryDatabase("CarRentalDb"));
                    });
                });

            _client = _factory.CreateClient();
        }

        [Theory]
        [JsonData<string>(CAR_CONTROLLER_TEST_FOLDER + "GetAllCarsTestData.json", "ValidInput")]
        public async Task GetAllCars_ForValidModel_ReturnsOk(string query)
        {
            var response = await _client.GetAsync("api/cars?" + query);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [JsonData<string>(CAR_CONTROLLER_TEST_FOLDER + "GetAllCarsTestData.json", "InvalidInput")]
        public async Task GetAllCars_ForInvalidModel_ReturnsBadRequest(string query)
        {
            var response = await _client.GetAsync("api/cars?" + query);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetCar_ForValidId_ReturnsOk(int id)
        {
            SeedCars();
            var response = await _client.GetAsync("api/cars/" + id);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [JsonData<int>(CAR_CONTROLLER_TEST_FOLDER + "GetCarTestData.json", "InvalidInput")]
        public async Task GetCar_ForInvalidId_ReturnsNotFound(int id)
        {
            SeedCars();
            var response = await _client.GetAsync("api/cars/" + id);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddCar_ForValidModel_ReturnsCreated()
        {
            var model = new AddCarDto() { Brand = "Opel", Model = "Astra", Price = new Price() {} };
            var httpContent = model.ToHttpContent();

            var response = await _client.PostAsync("api/cars", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }



        private void SeedCars()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<CarRentalDbContext>();
            if (!dbContext.Cars.Any())
            {
                var car = new Car()
                {
                    Id = 1,
                    Brand = "Skoda",
                    Model = "Fabia I",
                    YearOfProduction = 2003,
                    Color = "Red",
                    HorsePower = 60,
                    Description = null,
                    Price = new Price()
                    {
                        PriceForAnHour = 60,
                        PriceForADay = 500,
                        PriceForAWeek = 3000
                    }
                };
                dbContext.Cars.Add(car);
                dbContext.SaveChanges();
            }
        }
    }
}

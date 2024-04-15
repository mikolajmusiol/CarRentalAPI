using CarRentalAPI.Entities;
using CarRentalAPI.IntegrationTests.Helpers;
using CarRentalAPI.Models.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CarRentalAPI.IntegrationTests.Controllers
{
    public class CarControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string CAR_CONTROLLER_TEST_FOLDER = "TestData/CarController/";
        private readonly HttpClient _client;
        private readonly Seeder _seeder;
        private readonly WebApplicationFactory<Program> _factory;

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
            _seeder = new Seeder(GetDbContext());
        }

        private CarRentalDbContext GetDbContext()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            return scope.ServiceProvider.GetService<CarRentalDbContext>();
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

        [Fact]
        public async Task GetCar_ForValidId_ReturnsOk()
        {
            var car = _seeder.GetCar();
            await _seeder.SeedCar(car);

            var response = await _client.GetAsync("api/cars/" + car.Id);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [JsonData<int>(CAR_CONTROLLER_TEST_FOLDER + "GetCarTestData.json", "InvalidInput")]
        public async Task GetCar_ForInvalidId_ReturnsNotFound(int id)
        {
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

        [Theory]
        [JsonData<AddCarDto>(CAR_CONTROLLER_TEST_FOLDER + "AddCarTestData.json", "InvalidInput")]
        public async Task AddCar_ForInvalidModel_ReturnsBadRequest(AddCarDto dto)
        {
            var httpContent = dto.ToHttpContent();

            var response = await _client.PostAsync("api/cars", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [JsonData<UpdateCarDto>(CAR_CONTROLLER_TEST_FOLDER + "UpdateCarTestData.json", "ValidInput")]
        public async Task UpdateCar_ForValidModel_ReturnsOk(UpdateCarDto dto)
        {
            var car = _seeder.GetCar();
            await _seeder.SeedCar(car);

            var httpContent = dto.ToHttpContent();

            var response = await _client.PutAsync("api/cars/" + car.Id, httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_ForNonExistingCar_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/cars/100000");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForExistingCar_ReturnsNoContent()
        {
            var car = _seeder.GetCar();
            await _seeder.SeedCar(car);

            var response = await _client.DeleteAsync("/api/cars/" + car.Id);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}

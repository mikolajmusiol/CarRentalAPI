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
    public class OrderControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string ORDER_CONTROLLER_TEST_FOLDER = "TestData/OrderController/";
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Seeder _seeder;

        public OrderControllerTests(WebApplicationFactory<Program> factory)
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
            _seeder.SeedUser(_seeder.GetUser());
        }

        private CarRentalDbContext GetDbContext()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            return scope.ServiceProvider.GetService<CarRentalDbContext>();
        }

        [Theory]
        [JsonData<string>(ORDER_CONTROLLER_TEST_FOLDER + "GetAllOrdersTestData.json", "ValidInput")]
        public async Task GetAllOrders_ForValidModel_ReturnsOk(string query)
        {
            var response = await _client.GetAsync("api/orders?" + query);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [JsonData<string>(ORDER_CONTROLLER_TEST_FOLDER + "GetAllOrdersTestData.json", "InvalidInput")]
        public async Task GetAllOrders_ForInvalidModel_ReturnsBadRequest(string query)
        {
            var response = await _client.GetAsync("api/orders?" + query);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetOrder_ForValidId_ReturnsOk()
        {
            Order order = _seeder.GetOrder();
            await _seeder.SeedOrder(order);

            var response = await _client.GetAsync("api/orders/" + order.Id);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [JsonData<int>(ORDER_CONTROLLER_TEST_FOLDER + "GetOrderTestData.json", "InvalidInput")]
        public async Task GetOrder_ForInvalidId_ReturnsNotFound(int id)
        {
            var response = await _client.GetAsync("api/orders/" + id);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateOrder_ForValidModel_ReturnsCreated()
        {
            var car = _seeder.GetCar();
            await _seeder.SeedCar(car);
            var model = new CreateOrderDto() { CarId = car.Id, RentalFrom = DateTime.Now, RentalTo = DateTime.Now.AddDays(1) };
            var httpContent = model.ToHttpContent();

            var response = await _client.PostAsync("api/orders", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Theory]
        [JsonData<CreateOrderDto>(ORDER_CONTROLLER_TEST_FOLDER + "CreateOrderTestData.json", "InvalidInput")]
        public async Task CreateOrder_ForInvalidModel_ReturnsBadRequest(CreateOrderDto dto)
        {
            var car = _seeder.GetCar();
            await _seeder.SeedCar(car);
            dto.CarId = car.Id;

            var httpContent = dto.ToHttpContent();

            var response = await _client.PostAsync("api/orders", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [JsonData<UpdateOrderDto>(ORDER_CONTROLLER_TEST_FOLDER + "UpdateOrderTestData.json", "ValidInput")]
        public async Task UpdateOrder_ForValidModel_ReturnsOk(UpdateOrderDto dto)
        {
            var order = _seeder.GetOrder();
            await _seeder.SeedOrder(order);
            var httpContent = dto.ToHttpContent();

            var response = await _client.PutAsync("api/orders/" + order.Id, httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_ForNonExistingOrder_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/cars/100000");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForExistingOrder_ReturnsNoContent()
        {
            var order = _seeder.GetOrder();
            await _seeder.SeedOrder(order);
            var response = await _client.DeleteAsync("/api/cars/" + order.Id);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}

using CarRentalAPI.Entities;
using CarRentalAPI.IntegrationTests.Helpers;
using CarRentalAPI.Models.Dtos;
using CarRentalAPI.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace CarRentalAPI.IntegrationTests.Controllers
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();
        private const string ACCOUNT_CONTROLLER_TEST_FOLDER = "TestData/AccountController/";

        public AccountControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<CarRentalDbContext>));
                        services.Remove(dbContextOptions);

                        services.AddSingleton(_accountServiceMock.Object);

                        services.AddDbContext<CarRentalDbContext>(options => options.UseInMemoryDatabase("CarRentalDb"));
                    });
                });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            var registerUser = new RegisterUserDto()
            {
                Email = "unique@test.com",
                Password = "password",
                ConfirmPassword = "password",
                DateOfBirth = DateTime.Now,
                FirstName = "test",
                LastName = "test",
                City = "test",
                Street = "test",
                PostalCode = "12-123",
            };

            var httpContent = registerUser.ToHttpContent();

            var response = await _client.PostAsync("/api/account/register", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [JsonData<RegisterUserDto>(ACCOUNT_CONTROLLER_TEST_FOLDER + "RegisterUserTestData.json", "InvalidInput")]
        public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest(RegisterUserDto dto)
        {
            var httpContent = dto.ToHttpContent();

            var response = await _client.PostAsync("/api/account/register", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ForRegisteredUser_ReturnsOk()
        {
            _accountServiceMock
                .Setup(e => e.GenerateJwt(It.IsAny<LoginDto>()))
                .Returns(Task.FromResult("jwt"));

            var loginDto = new LoginDto()
            {
                Email = "a@test.com",
                Password = "password"
            };

            var httpContent = loginDto.ToHttpContent();

            var response = await _client.PostAsync("/api/account/login", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}

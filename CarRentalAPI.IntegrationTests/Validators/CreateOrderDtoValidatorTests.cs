using CarRentalAPI.Entities;
using CarRentalAPI.IntegrationTests.Helpers;
using CarRentalAPI.Models.Dtos;
using CarRentalAPI.Models.Validators;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarRentalAPI.IntegrationTests.Validators
{
    public class CreateOrderDtoValidatorTests
    {
        private const string VALIDATORS_TEST_FOLDER = "TestData/Validators/";
        private readonly CarRentalDbContext _dbContext;
        private readonly Seeder _seeder;

        public CreateOrderDtoValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<CarRentalDbContext>();
            builder.UseInMemoryDatabase("CarRentalDb");
            _dbContext = new CarRentalDbContext(builder.Options);
            _seeder = new Seeder(_dbContext);
        }

        [Fact]
        public async Task Validate_ForCorrectModel_ReturnsSuccess()
        {
            var order = _seeder.GetOrder();
            await _seeder.SeedOrder(order);

            var model = new CreateOrderDto()
            {
                CarId = order.CarId,
                RentalFrom = order.RentalTo.AddDays(1),
                RentalTo = order.RentalTo.AddDays(2)
            };
            
            var validator = new CreateOrderDtoValidator(_dbContext);
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [JsonData<CreateOrderDto>(VALIDATORS_TEST_FOLDER + "CreateOrderDtoTestData.json", "InvalidInput")]
        public async Task Validate_ForIncorrectModel_ReturnsFailure(CreateOrderDto model)
        {
            var order = _seeder.GetOrder();
            await _seeder.SeedOrder(order);

            var validator = new CreateOrderDtoValidator(_dbContext);
            var result = validator.TestValidate(model);
            result.ShouldHaveAnyValidationError();
        }

        [Theory]
        [JsonData<CreateOrderDto>(VALIDATORS_TEST_FOLDER + "CreateOverlappingTestData.json", "InvalidInput")]
        public async Task Validate_ForOverlappingRentals_ReturnsFailure(CreateOrderDto model)
        {
            var order = _seeder.GetOrder();
            order.RentalFrom = DateTime.Parse("2024-01-10");
            order.RentalTo = DateTime.Parse("2024-01-20");
            await _seeder.SeedOrder(order);
            model.CarId = order.CarId;

            var validator = new CreateOrderDtoValidator(_dbContext);
            var result = validator.TestValidate(model);
            result.ShouldHaveAnyValidationError();
        }
    }
}

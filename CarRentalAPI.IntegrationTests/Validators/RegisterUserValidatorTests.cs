using CarRentalAPI.Entities;
using CarRentalAPI.IntegrationTests.Helpers;
using CarRentalAPI.Models.Dtos;
using CarRentalAPI.Models.Validators;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarRentalAPI.IntegrationTests.Validators
{
    public class RegisterUserValidatorTests
    {
        private readonly CarRentalDbContext _dbContext;
        private readonly Seeder _seeder;
        private const string VALIDATORS_TEST_FOLDER = "TestData/Validators/";

        public RegisterUserValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<CarRentalDbContext>();
            builder.UseInMemoryDatabase("CarRentalDb");
            _dbContext = new CarRentalDbContext(builder.Options);
            _seeder = new Seeder(_dbContext);
        }

        [Fact]
        public void Validate_ForValidModel_ReturnsSuccess()
        {
            var model = new RegisterUserDto()
            {
                Email = "testValidator@test.pl",
                Password = "password",
                ConfirmPassword = "password",
                DateOfBirth = DateTime.Now
            };

            var validator = new RegisterUserDtoValidator(_dbContext);

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [JsonData<RegisterUserDto>(VALIDATORS_TEST_FOLDER + "RegisterUserValidatorTestData.json", "InvalidInput")]
        public async Task Validate_ForInvalidModel_ReturnsFailure(RegisterUserDto model)
        {
            var user = _seeder.GetUser();
            await _seeder.SeedUser(user);
            var validator = new RegisterUserDtoValidator(_dbContext);

            var result = validator.TestValidate(model);

            result.ShouldHaveAnyValidationError();
        }
    }
}

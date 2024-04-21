using CarRentalAPI.IntegrationTests.Helpers;
using CarRentalAPI.Models;
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
    public class QueryModelValidatorTests
    {
        private const string VALIDATORS_TEST_FOLDER = "TestData/Validators/";

        [Theory]
        [JsonData<QueryModel>(VALIDATORS_TEST_FOLDER + "QueryModelTestData.json", "ValidInput")]
        public void Validate_ForCorrectModel_ReturnsSuccess(QueryModel model)
        {
            var validator = new QueryModelValidator();
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [JsonData<QueryModel>(VALIDATORS_TEST_FOLDER + "QueryModelTestData.json", "InvalidInput")]
        public void Validate_ForIncorrectModel_ReturnsFailure(QueryModel model)
        {
            var validator = new QueryModelValidator();
            var result = validator.TestValidate(model);
            result.ShouldHaveAnyValidationError();
        }
    }
}

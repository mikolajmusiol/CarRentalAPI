using CarRentalAPI.UnitTests.Helpers;
using CarRentalAPI.Utilities;
using FluentAssertions;
using System.Text.Json.Serialization;
using Xunit;

namespace CarRentalAPI.UnitTests.UtilitiesTests
{
    public class CheckOverlapTests
    {
        private const string CHECK_OVERLAP_TEST_FOLDER = "TestData/UtilitiesTests/CheckOverlapTests/";

        [Theory]
        [JsonData(CHECK_OVERLAP_TEST_FOLDER + "ForOverlappingInput.json")]
        public void OfTwoTimePeriods_ForOverlappingInput_ReturnsTrue(DateTime AStart,
            DateTime AEnd, DateTime BStart, DateTime BEnd)
        {
            var result = CheckOverlap.OfTwoTimePeriods(AStart, AEnd, BStart, BEnd);
            result.Should().BeTrue();
        }

        [Theory]
        [JsonData(CHECK_OVERLAP_TEST_FOLDER + "ForNotOverlappingInput.json")]
        public void OfTwoTimePeriods_ForNotOverlappingInput_ReturnsFalse(DateTime AStart,
            DateTime AEnd, DateTime BStart, DateTime BEnd)
        {
            var result = CheckOverlap.OfTwoTimePeriods(AStart, AEnd, BStart, BEnd);
            result.Should().BeFalse();
        }
    }
}

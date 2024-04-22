using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.UnitTests.TestData.UtilitiesTests.OrderValueCalculatorTests;
using CarRentalAPI.Utilities;
using CarRentalAPI.Utilities.Interfaces;
using FluentAssertions;
using Xunit;

namespace CarRentalAPI.UnitTests.UtilitiesTests
{
    public class OrderValueCalculatorTests
    {
        private readonly IOrderValueCalculator _valueCalculator;

        public OrderValueCalculatorTests()
        {
            _valueCalculator = new OrderValueCalculator();
        }

        [Theory]
        [MemberData(nameof(OrderValueCalculatorTestData.GetPriceForAnHourData), MemberType = typeof(OrderValueCalculatorTestData))]
        public void CalculateOrderValue_ForExistingPriceForAnHour_ReturnsOrderValue(Order order, Price price)
        {
            var result = _valueCalculator.CalculateOrderValue(order, price);
            result.Should().Be(15);
        }

        [Theory]
        [MemberData(nameof(OrderValueCalculatorTestData.GetPriceForADayData), MemberType = typeof(OrderValueCalculatorTestData))]
        public void CalculateOrderValue_ForExistingPriceForADay_ReturnsOrderValue(Order order, Price price)
        {
            var result = _valueCalculator.CalculateOrderValue(order, price);
            result.Should().Be(20);
        }

        [Theory]
        [MemberData(nameof(OrderValueCalculatorTestData.GetPriceForAWeekData), MemberType = typeof(OrderValueCalculatorTestData))]
        public void CalculateOrderValue_ForExistingPriceForAWeek_ReturnsOrderValue(Order order, Price price)
        {
            var result = _valueCalculator.CalculateOrderValue(order, price);
            result.Should().Be(50);
        }

        [Theory]
        [MemberData(nameof(OrderValueCalculatorTestData.GetEmptyPrice), MemberType = typeof(OrderValueCalculatorTestData))]
        public void CalculateOrderValue_ForNonExistentPrices_ThrowsException(Order order, Price price)
        {
            Action result = () => _valueCalculator.CalculateOrderValue(order, price);
            result.Should().Throw<NotFoundException>();
        }
    }
}

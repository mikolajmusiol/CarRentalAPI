using CarRentalAPI.Entities;

namespace CarRentalAPI.UnitTests.TestData.UtilitiesTests.OrderValueCalculatorTests
{
    public class OrderValueCalculatorTestData
    {
        public static IEnumerable<object[]> GetPriceForAnHourData()
        {
            yield return new object[]
            {
                new Order()
                {
                    RentalFrom = new DateTime(2000,01,01,12,00,00),
                    RentalTo = new DateTime(2000,01,01,15,00,00) },
                new Price()
                {
                    PriceForAnHour = 5
                }
            };
        }

        public static IEnumerable<object[]> GetPriceForADayData()
        {
            yield return new object[]
            {
                new Order()
                {
                    RentalFrom = new DateTime(2000,01,01),
                    RentalTo = new DateTime(2000,01,05) },
                new Price()
                {
                    PriceForADay = 5
                }
            };
        }

        public static IEnumerable<object[]> GetPriceForAWeekData()
        {
            yield return new object[]
            {
                new Order()
                {
                    RentalFrom = new DateTime(2000,01,01),
                    RentalTo = new DateTime(2000,01,08) },
                new Price()
                {
                    PriceForAWeek = 50
                }
            };
        }

        public static IEnumerable<object[]> GetEmptyPrice()
        {
            yield return new object[]
            {
                new Order()
                {
                    RentalFrom = new DateTime(2000,01,01),
                    RentalTo = new DateTime(2000,01,08) },
                new Price()
                {

                }
            };
        }
    }
}

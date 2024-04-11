using CarRentalAPI.Entities;
using CarRentalAPI.Models.Dtos;
using System.Collections;

namespace CarRentalAPI.IntegrationTests.TestData.CarController
{
    public class UpdateCarValidTestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>()
    {
        new object[] { 1, new UpdateCarDto()
        {
            Price = new Price() {PriceForADay = 10},
            Mileage = 10,
            HorsePower = 10
        }},
        new object[] { 1, new UpdateCarDto()
        {
            Description = "New description",
            Mileage = 100000,
            Color = "Black"
        }}
    };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

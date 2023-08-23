using CarRentalAPI.Entities;

namespace CarRentalAPI.Models
{
    public class OrderDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Value { get; set; }
        public TimeSpan RentalPeriod { get; set; }
        public List<Car> Cars { get; set; }
    }
}

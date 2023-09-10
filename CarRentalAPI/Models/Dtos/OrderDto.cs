using CarRentalAPI.Entities;

namespace CarRentalAPI.Models.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Value { get; set; }
        public DateTime RentalFrom { get; set; }
        public DateTime RentalTo { get; set; }
        public Car Car { get; set; }
    }
}

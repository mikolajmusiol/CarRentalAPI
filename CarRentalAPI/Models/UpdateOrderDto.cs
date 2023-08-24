using CarRentalAPI.Entities;

namespace CarRentalAPI.Models
{
    public class UpdateOrderDto
    {
        public DateTime? RentalFrom { get; set; }
        public DateTime? RentalTo { get; set; }
        public Car? Car { get; set; }
    }
}
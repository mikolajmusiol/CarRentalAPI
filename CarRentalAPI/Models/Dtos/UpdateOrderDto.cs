using CarRentalAPI.Entities;

namespace CarRentalAPI.Models.Dtos
{
    public class UpdateOrderDto
    {
        public DateTime? RentalFrom { get; set; }
        public DateTime? RentalTo { get; set; }
        public int? CarId { get; set; }
    }
}
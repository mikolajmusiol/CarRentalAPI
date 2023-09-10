using CarRentalAPI.Entities;

namespace CarRentalAPI.Models.Dtos
{
    public class UpdateCarDto
    {
        public int? Mileage { get; set; }
        public string? Color { get; set; }
        public int? HorsePower { get; set; }
        public string? Description { get; set; }
        public Price? Price { get; set; }
    }
}

using CarRentalAPI.Entities;
using System.Text.Json.Serialization;

namespace CarRentalAPI.Models
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int YearOfProduction { get; set; }
        public int? Mileage { get; set; }
        public string? Color { get; set; }
        public int HorsePower { get; set; }
        public string? Description { get; set; }

        public Price Price { get; set; }
    }
}

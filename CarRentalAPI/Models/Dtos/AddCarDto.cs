using CarRentalAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models.Dtos
{
    public class AddCarDto
    {
        [Required]
        [MaxLength(25)]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        public int YearOfProduction { get; set; }
        public int? Mileage { get; set; }
        public string? Color { get; set; }
        public int HorsePower { get; set; }
        public string? Description { get; set; }

        [Required]
        public Price Price { get; set; }
    }
}

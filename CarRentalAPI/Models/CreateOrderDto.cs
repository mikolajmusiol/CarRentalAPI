using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models
{
    public class CreateOrderDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public DateTime RentalFrom { get; set; }
        [Required]
        public DateTime RentalTo { get; set; }
    }
}

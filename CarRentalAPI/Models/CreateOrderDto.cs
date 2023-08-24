using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models
{
    public class CreateOrderDto
    {
        [Required]
        public string Email { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime RentalFrom { get; set; }
        public DateTime RentalTo { get; set; }
    }
}

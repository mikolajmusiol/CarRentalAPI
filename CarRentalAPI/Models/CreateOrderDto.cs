using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models
{
    public class CreateOrderDto
    {
        [Required]
        public int CarId { get; set; }
        [Required]
        public DateTime RentalFrom { get; set; }
        [Required]
        public DateTime RentalTo { get; set; }
    }
}

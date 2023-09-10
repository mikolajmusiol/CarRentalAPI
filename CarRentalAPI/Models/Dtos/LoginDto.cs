using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models.Dtos
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
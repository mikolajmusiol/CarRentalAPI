using CarRentalAPI.Models;

namespace CarRentalAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Task RegisterUser(RegisterUserDto dto);
        Task<string> GenerateJwt(LoginDto dto);
    }
}

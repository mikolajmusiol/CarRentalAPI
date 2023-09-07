using System.Security.Claims;

namespace CarRentalAPI.Services.Interfaces
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal User { get; }
    }
}

using CarRentalAPI.Services.Interfaces;
using System.Security.Claims;

namespace CarRentalAPI.Services
{
    public class UserContextService : IUserContextService
    {
        public ClaimsPrincipal User => _accessor.HttpContext?.User;
        public int? GetUserId => User is null ? null : (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

        private readonly IHttpContextAccessor _accessor;

        public UserContextService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }
}
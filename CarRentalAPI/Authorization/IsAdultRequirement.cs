using Microsoft.AspNetCore.Authorization;

namespace CarRentalAPI.Authorization
{
    public class IsAdultRequirement : IAuthorizationRequirement
    {
        public int MinimumAge => 18;
    }
}

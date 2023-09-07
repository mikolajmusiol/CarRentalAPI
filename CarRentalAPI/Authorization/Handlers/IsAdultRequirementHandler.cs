using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace CarRentalAPI.Authorization.Handlers
{
    public class IsAdultRequirementHandler : AuthorizationHandler<IsAdultRequirement>
    {
        private readonly ILogger<IsAdultRequirementHandler> _logger;

        public IsAdultRequirementHandler(ILogger<IsAdultRequirementHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdultRequirement requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(u => u.Type == ClaimTypes.DateOfBirth).Value);

            var userEmail = context.User.FindFirst(U => U.Type == ClaimTypes.Email).Value;

            if (dateOfBirth.AddYears(requirement.MinimumAge) < DateTime.Today)
            {
                _logger.LogInformation("Authorization succedded, user is an adult");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Authorization failed");
            }

            return Task.CompletedTask;
        }
    }
}

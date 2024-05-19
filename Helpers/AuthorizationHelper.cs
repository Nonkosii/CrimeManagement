using CrimeManagement.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CrimeManagement.Helpers
{
    public class AuthorizationHelper
    {
        // Helper method to check if the current user is the case manager or captain for the given case manager ID
        public bool IsCurrentUserCaseManagerOrCaptain(string caseManagerId, UserManager<ApplicationUser> userManager, ClaimsPrincipal user)
        {
            // Get the current user ID
            string? currentUserId = userManager.GetUserId(user);

            // Check if the current user is the case manager or captain for the given case manager ID
            return string.Equals(currentUserId, caseManagerId, StringComparison.OrdinalIgnoreCase)
                || user.IsInRole("Captain");
        }
    }
}

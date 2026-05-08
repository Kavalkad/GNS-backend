using GNS.Contracts.Requests.Interfaces;
using GNS.Exceptions;
using GNS.Services;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;

namespace GNS.Endpoints.Filters
{
    public class OwnerAccessToCyberClubFilter(IAuthService authService) : IEndpointFilter
    {
        private readonly IAuthService _authService = authService;
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next
            )
        {
            var userIdClaim = context.HttpContext.User
                .Claims.SingleOrDefault(c => c.Type == "Id");


            if (userIdClaim is null ||
                !Guid.TryParse(userIdClaim.Value, out Guid ownerId))
            {
                return Results.Unauthorized();
            }

            var cyberClubRequest = context.Arguments
                .OfType<ICyberClubRequest>()
                .FirstOrDefault();


            bool containsQueryId = context.HttpContext.Request.Query.TryGetValue("cyberClubId", out StringValues stringValue); 
            if (cyberClubRequest is null && !containsQueryId)
            {
                return Results.BadRequest("Request must contain cyber club identificators");
            }


            if (cyberClubRequest is not null)
            {
                bool isVerified = await _authService
                    .VerifyOwnerAccessToCyberClubAsync(ownerId, cyberClubRequest.CyberClubId);

                if (!isVerified)
                {
                    return Results.Forbid();
                }
            }

            if (containsQueryId)
            {
                if (string.IsNullOrEmpty(stringValue) || !Guid.TryParse(stringValue, out Guid cyberClubId))
                {
                    return Results.BadRequest("identificator from query must have Guid format");
                }
                bool isVerified = await _authService
                    .VerifyOwnerAccessToCyberClubAsync(ownerId, cyberClubId);

                if (!isVerified)
                {
                    return Results.Forbid();
                }
            }

            return await next(context);
        }
    }
}
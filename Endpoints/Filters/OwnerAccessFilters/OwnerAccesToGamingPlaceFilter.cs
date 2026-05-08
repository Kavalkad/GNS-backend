using GNS.Contracts.Requests.Interfaces;
using GNS.Exceptions;
using GNS.Services;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace GNS.Endpoints.Filters
{
    public class OwnerAccessToGamingPlaceFilter(IAuthService authService) : IEndpointFilter
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

            var request = context.Arguments
                .OfType<IGamingPlaceRequest>()
                .FirstOrDefault();

            var queryGamingPlaceId = context.HttpContext.Request.Query["gamingPlaceId"].ToString();

            if (request is null && queryGamingPlaceId is null)
            {
                return Results.BadRequest("Request must contain gaming place identificator");
            }


            if (request is not null)
            {
                bool isVerified = await _authService
                    .VerifyOwnerAccessToGamingPlaceAsync(ownerId, request.GamingPlaceId);

                if (!isVerified)
                {
                    return Results.Forbid();
                }
            }

            if (queryGamingPlaceId is not null)
            {
                if (!Guid.TryParse(queryGamingPlaceId, out Guid gamingPlaceId))
                {
                    return Results.BadRequest("identificator from query must have Guid format");
                }
                bool isVerified = await _authService
                    .VerifyOwnerAccessToGamingPlaceAsync(ownerId, gamingPlaceId);

                if (!isVerified)
                {
                    return Results.Forbid();
                }
            }

            return await next(context);
        }
    }
}
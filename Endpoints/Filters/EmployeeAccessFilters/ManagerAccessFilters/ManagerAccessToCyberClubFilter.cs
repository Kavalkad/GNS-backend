using GNS.Contracts.Requests.Interfaces;
using GNS.Services.Interfaces;


namespace GNS.Endpoints.Filters
{
    public class ManagerAccessToCyberClubFilter(IAuthService authService) : IEndpointFilter
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
                !Guid.TryParse(userIdClaim.Value, out Guid managerId))
            {
                return Results.Unauthorized();
            }


            var request = context.Arguments
                .OfType<ICyberClubRequest>()
                .FirstOrDefault();
            var queryCyberClubId = context.HttpContext.Request.Query["cyberClubId"].ToString();

            if (request is null && queryCyberClubId is null)
            {
                return Results.BadRequest("Can't get employeeId from request");
            }

            if (request is not null)
            {
                var cyberClubId = request.CyberClubId;

                bool isVerified = await _authService
                    .VerifyManagerAccessToCyberClubAsync(managerId, cyberClubId);
                if (!isVerified)
                {
                    return Results.Forbid();
                }
            }

            return await next(context);
        }
    }
}
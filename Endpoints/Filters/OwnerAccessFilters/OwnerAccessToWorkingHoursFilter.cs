using GNS.Contracts.Requests.Interfaces;
using GNS.Services.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class OwnerAccessToWorkingHoursFilter(IAuthService authService) : IEndpointFilter
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
                .OfType<IWorkingHoursRequest>()
                .FirstOrDefault();


            if (request is null)
            {
                return Results.BadRequest("Request must contain working hours identificator");
            }


            bool isVerified = await _authService
                .VerifyOwnerAccessToWorkingHoursAsync(ownerId, request.WorkingHoursId);

            if (!isVerified)
            {
                return Results.Forbid();
            }

            return await next(context);
        }
    }
}
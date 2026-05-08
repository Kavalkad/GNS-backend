using GNS.Contracts.Requests.Interfaces;
using GNS.Exceptions;
using GNS.Services;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace GNS.Endpoints.Filters
{
    public class ManagerAccessToEmployeeFilter(IAuthService authService) : IEndpointFilter
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
                .OfType<IEmployeeRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                return Results.BadRequest("Can't get employeeId from request");
            }

            var employeeId = request.EmployeeId;

            bool isVerified = await _authService.VerifyManagerAccessToEmployeeAsync(managerId, employeeId);
            if (!isVerified)
            {
                return Results.Forbid();
            }

            return await next(context);
        }
    }
}
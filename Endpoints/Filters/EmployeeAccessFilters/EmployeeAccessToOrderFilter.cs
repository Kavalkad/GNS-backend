using GNS.Contracts.Requests.Interfaces;
using GNS.Services.Interfaces;


namespace GNS.Endpoints.Filters
{
    public class EmployeeAccessToOrderFilter(IAuthService authService) : IEndpointFilter
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
                !Guid.TryParse(userIdClaim.Value, out Guid employeeId))
            {
                return Results.Unauthorized();
            }

           
            var request = context.Arguments
                .OfType<IOrderRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                return Results.BadRequest("Can't get employeeId from request");
            }

            bool isVerified = await _authService.VerifyEmployeeAccessToGamingPlaceAsync(employeeId, request.OrderId);
            if (!isVerified)
            {
                return Results.Forbid();
            }

            return await next(context);
        }
    }
}

using System.Security.Claims;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
using GNS.Services.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class RoleFilter : IEndpointFilter
    {
        private readonly Role _role;
        public RoleFilter(Role role)
        {
            _role = role;
        }
        
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var userStringId = context.HttpContext.User.FindFirstValue("Id");

            if (!Guid.TryParse(userStringId, out Guid userId))
            {
                return Results.BadRequest("UserId has incorrect format");
            }

            var usersRepository = context.HttpContext.RequestServices.GetRequiredService<IUsersRepository>();
            var user = await usersRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return Results.BadRequest("User data is not exists");
            }
            
            return user.Role == _role ? await next(context) : Results.Forbid();
        }
    }
}
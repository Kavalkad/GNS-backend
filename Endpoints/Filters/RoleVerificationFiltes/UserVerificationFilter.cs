
using System.Security.Claims;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
using GNS.Services.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class UserVerificationFilter : IEndpointFilter
    {
        private readonly IUsersRepository _usersRepository;
        public UserVerificationFilter(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var userStringId = context.HttpContext.User.FindFirstValue("Id");

            if (Guid.TryParse(userStringId, out Guid userId))
            {
                return Results.BadRequest("UserId has incorrect format");
            }
            var user = await _usersRepository.GetById(userId);

            if (user is null)
            {
                return Results.BadRequest("User data is not exists");
            }
            var isUser = user.Role == Role.User;

            if (!isUser)
            {
                return Results.Forbid();
            }
            
            return await next(context);
        }
    }
}
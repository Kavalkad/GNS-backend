using System.Security.Claims;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
using GNS.Services.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class OwnerVerificationFilter : IEndpointFilter
    {

        private readonly IOwnersRepository _ownersRepository;
        public OwnerVerificationFilter(IOwnersRepository ownersRepository)
        {
            _ownersRepository = ownersRepository;
        }
        
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var ownerStringId = context.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Id")
                    ?? throw new Exception("Failed to read id claim");

            if (Guid.TryParse(ownerStringId.Value, out Guid ownerId))
            {
                return Results.BadRequest("Id has incorrect format");
            }
            var owner = await _ownersRepository.GetById(ownerId);

            if (owner is null)
            {
                return Results.BadRequest("Owner data doesn't exists");
            }
            
            return await next(context);
        }
    }
}
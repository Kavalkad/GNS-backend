using System.Security.Claims;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
using GNS.Services.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class OwnerVerificationFilter : IEndpointFilter
    {
        private readonly IVerificationService _verificationService;
        private readonly IOwnersRepository _ownersRepository;
        public OwnerVerificationFilter(
            IVerificationService verificationService,
            IOwnersRepository ownersRepository
            )
        {
            _verificationService = verificationService;
            _ownersRepository = ownersRepository;
        }
        
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var ownerStringId = context.HttpContext.User.FindFirstValue("Id");

            if (Guid.TryParse(ownerStringId, out Guid ownerId))
            {
                return Results.BadRequest("Id has incorrect format");
            }
            var owner = await _ownersRepository.GetById(ownerId);

            if (owner is null)
            {
                return Results.BadRequest("Employee data doesn't exists");
            }
            
            return await next(context);
        }
    }
}
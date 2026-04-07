using GNS.Contracts;
using GNS.Contracts.Requests.Interfaces;
using GNS.Data;
using GNS.Data.Entities;
using GNS.Data.Repositories.Implementations;
using GNS.Exceptions;
using GNS.Extensions;
using GNS.Interfaces;
using GNS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Endpoints.Filters
{
    public class VerifyOwnerAccessFilter(ICyberClubService cyberClubService) : IEndpointFilter
    {
        private readonly ICyberClubService _cyberClubService = cyberClubService;

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next
            )
        {
            var request = context.Arguments
                .OfType<IIdRequest>()
                .FirstOrDefault()
                    ?? throw new Exception("Invalid request");
            
            var errors = new Dictionary<string, string[]>();

            if (context.HttpContext.Items.TryGetValue("ValidationErrors", out object? _errors))
            {
                errors = _errors as Dictionary<string, string[]>;
            }

            var ownerIdClaim = context.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Id")
                    ?? throw new Exception("Cannot find id in user claims");

            if (!Guid.TryParse(ownerIdClaim.Value, out Guid ownerId))
            {
                throw new IncorrectGuidException(ownerIdClaim.Value);
            }

            _ = Guid.TryParse(request.Id, out Guid cyberClubId);

            var ownersCyberClubs = await _cyberClubService.GetOwnerCyberClubsAsync(ownerId);
            if (ownersCyberClubs.Count == 0)
            {
                Results.Unauthorized();
                errors.Add("unauthorized", ["user don't have access to that cyberclub's data"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
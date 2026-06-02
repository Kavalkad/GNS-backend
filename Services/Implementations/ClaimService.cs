using System.Security.Claims;
using GNS.Enums;
using GNS.Interfaces;
using GNS.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace GNS.Services.Implementations
{
    public class ClaimService(IClaimsBuilder claimBuilder) : IClaimService
    {
        private readonly IClaimsBuilder _claimBuilder = claimBuilder;
        public ICollection<Claim> GenerateClaims(Guid userId, Role userRole)
        {
            _claimBuilder.AddIdClaim(userId);

            switch (userRole)
            {
                case Role.User:
                    _claimBuilder.AddUserClaim();
                    break;
                case Role.Admin:
                    _claimBuilder.AddAdminClaim();
                    goto case Role.User;
                case Role.Manager:
                    _claimBuilder.AddManagerClaim();
                    goto case Role.Admin;
                case Role.Owner:
                    _claimBuilder.AddOwnerClaim();
                    goto case Role.User;
            }

            return _claimBuilder.Build();

        }
    }
}
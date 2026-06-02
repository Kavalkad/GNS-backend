using System.Security.Claims;
using GNS.Enums;
using GNS.Interfaces;

namespace GNS.Services.Interfaces
{
    public interface IClaimService
    {
        ICollection<Claim> GenerateClaims(Guid userId, Role userRole);
    }
}
using System.Security.Claims;
using GNS.Enums;


namespace GNS.Services.Interfaces
{
    public interface IClaimService
    {
        ICollection<Claim> GenerateClaims(Guid userId, Role userRole);
    }
}
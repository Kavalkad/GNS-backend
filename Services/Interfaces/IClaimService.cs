using System.Security.Claims;
using GNS.Interfaces;

namespace GNS.Services.Interfaces
{
    public interface IClaimService
    {
        ICollection<Claim> GenerateClaims(IClaimsGeneratable entity);
    }
}
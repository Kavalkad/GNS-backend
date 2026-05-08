using System.Security.Claims;

namespace GNS.Services.Interfaces
{
    public interface IClaimsBuilder
    {
        void AddIdClaim(Guid id);
        void AddUserClaim();
        void AddAdminClaim();
        void AddManagerClaim();
        void AddOwnerClaim();
        ICollection<Claim> Build();
    }
}
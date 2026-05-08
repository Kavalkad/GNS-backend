using System.Security.Claims;
using GNS.Interfaces;
using GNS.Enums;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class ClaimsBuilder : IClaimsBuilder
    {
        private ICollection<Claim> Claims { get; } = [];

        public void AddIdClaim(Guid id)
        {
            Claims.Add(new Claim("Id", id.ToString()));
        }
        public void AddUserClaim()
        {
            Claims.Add(CustomClaims.UserClaim);
        }
        public void AddAdminClaim()
        {
            Claims.Add(CustomClaims.AdminClaim);
        }
        public void AddManagerClaim()
        {
            Claims.Add(CustomClaims.ManagerClaim);
        }
        public void AddOwnerClaim()
        {
            Claims.Add(CustomClaims.OwnerClaim);
        }

        public ICollection<Claim> Build()
        {
            return Claims;
        }
    }
}

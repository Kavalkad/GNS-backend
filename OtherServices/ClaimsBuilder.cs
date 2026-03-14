using System.Security.Claims;
using GNS.Interfaces;
using GNS.Enums;

namespace GNS.Services
{
    public static class ClaimsBuilder
    {
        private static Dictionary<Role, List<Claim>> RoleClaims { get; set; } = [];
            
        static ClaimsBuilder()
        {
            var roles = Enum.GetValues<Role>();
            
            for (int i = 0; i < roles.Length; i++)
            {
                var claims = new List<Claim>();

                for (int j = i; j >= 0; j--)
                {
                    claims.Add(CustomClaims.RoleClaim[roles[j]]);
                }
                RoleClaims.Add(roles[i], claims);
            }
        }
        public static List<Claim> GenerateClaims(IClaimsGeneratable entity)
        {
            var claims = new List<Claim>
            {
                new("Id", entity.Id.ToString())
            };

            claims.AddRange(RoleClaims[entity.Role]);
            
            return claims;
        }
    }
}

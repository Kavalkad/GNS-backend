using System.Security.Claims;
using GNS.Enums;

namespace GNS.Services
{
    public static class CustomClaims
    {
        public static Claim UserClaim { get;  } = new("User", "true");
        public static Claim AdminClaim { get; } = new("Admin", "true");
        public static Claim ManagerClaim { get;  } = new("Manager", "true");
        public static Claim OwnerClaim { get; } = new("Owner", "true");
        public static Dictionary<Role, Claim> RoleClaim { get; }
            = new Dictionary<Role, Claim>
            {
                { Role.User, UserClaim ?? throw new Exception("Userclaim is null") },
                { Role.Admin, AdminClaim ?? throw new Exception("Adminclaim is null") },
                { Role.Manager, ManagerClaim ?? throw new Exception("Managerclaim is null") },
                { Role.Owner, OwnerClaim ?? throw new Exception("Ownerclaim is null") },
            };

        



    }
}
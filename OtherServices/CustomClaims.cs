using System.Security.Claims;

namespace GNS.Services
{
    public static class CustomClaims
    {
        public static Claim UserClaim { get;  } = new("User", "true");
        public static Claim AdminClaim { get; } = new("Admin", "true");
        public static Claim ManagerClaim { get;  } = new("Manager", "true");
        public static Claim OwnerClaim { get; } = new("Owner", "true");
        
    }
}
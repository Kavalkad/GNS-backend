using GNS.Exceptions;
using Microsoft.AspNetCore.Authentication;

namespace GNS.Extensions
{
    public static class IHttpContextAccessorExtensions
    {
        public static Guid TryGetHttpUserId(this IHttpContextAccessor accessor)
        {
            var stringId = accessor.HttpContext.User
                .Claims.FirstOrDefault(c => c.Type == "Id").Value
                    ?? throw new AuthenticationFailureException("");
            if (!Guid.TryParse(stringId, out Guid result))
            {
                throw new IncorrectGuidException(stringId);
            }
            return result;
       }
    }
}
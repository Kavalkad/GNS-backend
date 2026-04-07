

namespace GNS.Extensions
{
    public static class IHttpContextAccessorExtensions
    {
        public static Guid TryGetHttpUserId(this IHttpContextAccessor accessor)
        {
            var stringId = accessor.HttpContext.User
                .Claims.FirstOrDefault(c => c.Type == "Id").Value
                    ?? throw new Exception("Id not found in claims");
            if (!Guid.TryParse(stringId, out Guid result))
            {
                throw new Exception("Incorrect Id format");
            }
            return result;
       }
    }
}
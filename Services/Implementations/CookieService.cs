using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class CookieService(IHttpContextAccessor contextAccessor) : ICookieService
    {
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        public void AppendCookie(string key, string value, CookieOptions options = default)
        {
            var context = _contextAccessor.HttpContext;
            if (context.Request.Cookies.ContainsKey(key))
            {
                context.Response.Cookies.Delete(key);
            }
            if (options is not null)
            {
                context.Response.Cookies.Append(key, value, options);
            }
            context.Response.Cookies.Append(key, value);
        }

        public void DeleteCookie(string key)
        {
            var context = _contextAccessor.HttpContext;
            context.Response.Cookies.Delete(key);
        }
    }
}
namespace GNS.Services.Interfaces
{
    public interface ICookieService
    {
        void AppendCookie(string key, string value, CookieOptions options = default!);
        void DeleteCookie(string key);
    }
}
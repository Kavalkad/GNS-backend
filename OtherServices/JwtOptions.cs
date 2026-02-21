namespace GNS.Services
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = null!;
        public int ExpiredHours { get; set; }
    }   
}
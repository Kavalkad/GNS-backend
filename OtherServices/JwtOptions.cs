namespace GNS.Services
{
    public class JwtOptions
    {
        public bool ValidateIssuer { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public bool ValidateAudience { get; set; }
        public string SecretKey { get; set; } = null!;
        public int AccessTokenValidityMins { get; set; }
        public int RefreshTokenValidityDays { get; set; }
    }
}
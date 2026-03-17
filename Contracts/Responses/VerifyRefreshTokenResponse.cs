using GNS.Data.Entities;

namespace GNS.Contracts.Responses
{
    public record class VerifyRefreshTokenResponse
    {
        public bool IsValid { get; set; }
        public RefreshTokenEntity? NewRefreshToken { get; set; }
    }
    
}
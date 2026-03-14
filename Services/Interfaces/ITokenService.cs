using GNS.Data.Entities;
using GNS.Interfaces;


namespace GNS.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IClaimsGeneratable entity);
        Task<RefreshTokenEntity> GenerateRefreshToken(Guid userId);
        Task<List<RefreshTokenEntity>> GetByUserId(Guid userId);
        Task RevokeRefreshToken(string refreshTokenValue);
    }
}
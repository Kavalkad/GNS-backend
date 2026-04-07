using GNS.Data.Entities;
using GNS.Interfaces;


namespace GNS.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IClaimsGeneratable entity);
        Task<RefreshTokenEntity> GenerateRefreshTokenAsync(Guid userId, CancellationToken token = default);
        Task<List<RefreshTokenEntity>> GetByUserIdAsync(Guid userId, CancellationToken token = default);
        Task RevokeRefreshTokenAsync(string refreshTokenValue, CancellationToken token = default);
    }
}
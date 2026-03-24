using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IRefreshTokensRepository
    {
        Task AddAsync(RefreshTokenEntity refreshToken);
        Task<RefreshTokenEntity> GetByValue(Guid token);
        Task<List<RefreshTokenEntity>> GetTokensByUserId(Guid userId);
        Task RevokeRefreshToken(string tokenValue);
    }
}
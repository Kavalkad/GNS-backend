using GNS.Data.Entities;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IRefreshTokensRepository
    {
        Task AddAsync(RefreshTokenEntity refreshToken);
        Task<RefreshTokenEntity> GetByHash(string tokenHash);
        Task<List<RefreshTokenEntity>> GetTokensByUserId(Guid userId);
        Task UpdateRefreshToken(string tokenValue);
    }
}
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class RefreshTokensRepository : IRefreshTokensRepository
    {
        private readonly AppDbContext _dbcontext;
        public RefreshTokensRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddAsync(RefreshTokenEntity refreshToken)
        {
            await _dbcontext.RefreshTokens.AddAsync(refreshToken);
        }

        // !!!!!!!!!!!!!!!!!
        public async Task<RefreshTokenEntity> GetByValue(Guid token)
        {
            return await _dbcontext.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.Token == token)
                    ?? throw new Exception($"RefreshToken with value: {token} not found");
        }

        public async Task<List<RefreshTokenEntity>> GetTokensByUserId(Guid userId)
        {
            return await _dbcontext.RefreshTokens
                .AsNoTracking()
                .Where(rt => rt.UserId == userId)
                .ToListAsync();
        }
        public async Task RevokeRefreshToken(string tokenValue)
        {
            await _dbcontext.RefreshTokens
                .Where(rt => rt.Token.ToString() == tokenValue)
                .ExecuteUpdateAsync(ub =>
                {
                    ub.SetProperty(rt => rt.RevokedAt, DateTime.UtcNow);
                });
        }
    }
}
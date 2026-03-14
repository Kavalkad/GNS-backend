

using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class GameGPsRepository : IGameGPsRepository
    {
        private readonly AppDbContext _dbcontext;
        public GameGPsRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task AddPairs(params GameGamingPlaceEntity[] pairs)
        {
            await _dbcontext.GameGamingPlaces.AddRangeAsync(pairs);
        }
        public async Task DeletePairs(Guid gameId, IEnumerable<Guid> gamingPlaceIds)
        {
            await _dbcontext.GameGamingPlaces
                .Where(p => p.GameId == gameId && gamingPlaceIds.Contains(p.GamingPlaceId))
                .ExecuteDeleteAsync();
                
        }

    }
}
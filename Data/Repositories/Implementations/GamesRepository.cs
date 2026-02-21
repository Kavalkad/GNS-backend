using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class GamesRepository : IGamesRepository
    {
        private readonly AppDbContext _dbcontext;
        public GamesRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task Add(string title)
        {
            var game = new GameEntity
            {
                Title = title
            };
            await _dbcontext.Games.AddAsync(game);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<List<GameEntity>> GetByFilter(string filter)
        {
            return await _dbcontext.Games
                .AsNoTracking()
                .Where(g => g.Title.Contains(filter))
                .ToListAsync();
        }
        public async Task Update(Guid gameId, string newTitle)
        {
            await _dbcontext.Games
                .Where(g => g.Id == gameId)
                .ExecuteUpdateAsync(ub =>
                {
                    ub.SetProperty(g => g.Title, newTitle);
                });
        }
        public async Task Delete(Guid gameId)
        {
            await _dbcontext.Games
                .Where(g => g.Id == gameId)
                .ExecuteDeleteAsync();
        }
    }
}
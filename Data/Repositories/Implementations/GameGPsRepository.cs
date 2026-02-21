

using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;

namespace GNS.Data.Repositories.Implementations
{
    public class GameGPsRepository : IGameGPsRepository
    {
        private readonly AppDbContext _dbcontext;
        public GameGPsRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task Create(GameEntity game, GamingPlaceEntity[] gamingPlaces)
        {
            var gameGamingPlaces = gamingPlaces
                .Select(gp => new GameGamingPlaceEntity
                {
                    GameId = game.Id,
                    GamingPlaceId = gp.Id
                })
                .ToArray();

            await _dbcontext.AddRangeAsync(gameGamingPlaces);
        }

    }
}
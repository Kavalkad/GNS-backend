using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;


namespace GNS.Data.Repositories.Implementations
{
    public class GamesRepository(AppDbContext dbcontext) 
        : BaseRepository<GameEntity>(dbcontext), IGamesRepository
    {
    }
}
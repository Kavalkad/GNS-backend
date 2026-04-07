using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class GamesRepository(AppDbContext dbcontext) 
        : BaseRepository<GameEntity>(dbcontext), IGamesRepository
    {
    }
}
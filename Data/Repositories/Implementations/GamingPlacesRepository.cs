using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
using GNS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class GamingPlacesRepository : BaseRepository<GamingPlaceEntity>, IGamingPlacesRepository
    {
        public GamingPlacesRepository(AppDbContext dbcontext) : base(dbcontext) { }

        
    }
}
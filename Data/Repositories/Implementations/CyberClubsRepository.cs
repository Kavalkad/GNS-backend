using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class CyberClubsRepository : BaseRepository<CyberClubEntity>, ICyberClubsRepository
    {

        public CyberClubsRepository(AppDbContext dbcontext) : base(dbcontext)
        {

        }
        // Update
       

        // Delete

       


    }
}
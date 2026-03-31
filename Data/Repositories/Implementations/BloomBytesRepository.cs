using System.Linq.Expressions;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class BloomBytesRepository : BaseRepository<BloomBytesEntity>, IBloomBytesRepository
    {
        public BloomBytesRepository(AppDbContext dbcontext) : base(dbcontext)
        {

        }

        
    }
}
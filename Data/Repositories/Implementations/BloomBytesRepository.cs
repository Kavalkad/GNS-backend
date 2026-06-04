using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;

namespace GNS.Data.Repositories.Implementations
{
    public class BloomBytesRepository(AppDbContext dbcontext) 
        : BaseRepository<BloomBytesEntity>(dbcontext), IBloomBytesRepository
    {
    }
}
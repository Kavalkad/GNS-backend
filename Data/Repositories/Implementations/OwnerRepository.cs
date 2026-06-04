using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;


namespace GNS.Data.Repositories.Implementations
{
    public class OwnersRepository(AppDbContext dbcontext)
        : BaseRepository<OwnerEntity>(dbcontext), IOwnersRepository
    {
        
    }
}
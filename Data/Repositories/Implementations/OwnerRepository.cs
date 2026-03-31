using GNS.Enums;
using Microsoft.EntityFrameworkCore;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;
using System.Runtime.CompilerServices;

namespace GNS.Data.Repositories.Implementations
{
    public class OwnersRepository(AppDbContext dbcontext)
        : BaseRepository<OwnerEntity>(dbcontext), IOwnersRepository
    {
        
    }
}
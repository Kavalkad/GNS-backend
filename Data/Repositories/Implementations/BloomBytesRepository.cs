using System.Linq.Expressions;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class BloomBytesRepository(AppDbContext dbcontext) 
        : BaseRepository<BloomBytesEntity>(dbcontext), IBloomBytesRepository
    {
    }
}
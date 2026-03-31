using GNS.Enums;
using Microsoft.EntityFrameworkCore;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;

namespace GNS.Data.Repositories.Implementations
{
    public class WorkingHoursRepository(AppDbContext dbcontext) 
        : BaseRepository<WorkingHoursEntity>(dbcontext), IWorkingHoursRepository
    {
        
    }
}
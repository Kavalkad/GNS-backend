using GNS.Enums;
using Microsoft.EntityFrameworkCore;
using GNS.Data.Repositories.Interfaces;
using GNS.Data.Entities;

namespace GNS.Data.Repositories.Implementations
{
    public class WorkingHoursRepository : IWorkingHoursRepository
    {
        private readonly AppDbContext _dbcontext;
        public WorkingHoursRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task CreateWorkingHours(
            Guid cyberClubId,
            CustomDayOfWeek dayOfWeek,
            TimeOnly startHour,
            TimeOnly endHour,
            bool isOpen
            )
        {
            var workingHours = new WorkingHoursEntity
            {
                CyberClubId = cyberClubId,
                DayOfWeek = dayOfWeek,
                StartHour = startHour,
                EndHour = endHour,
                IsOpen = isOpen
            };
            await _dbcontext.WorkingHours.AddAsync(workingHours);
            await _dbcontext.SaveChangesAsync();
        }


        public async Task<List<WorkingHoursEntity>> GetWorkingHoursAsync(Guid cyberClubId)
        {
            return await _dbcontext.WorkingHours
                .AsNoTracking()
                .Where(wh => wh.CyberClubId == cyberClubId)
                .ToListAsync();
        }
        public async Task<WorkingHoursEntity> GetDayWorkingHoursAsync(Guid cyberClubId, CustomDayOfWeek dayOfWeek)
        {
            return await _dbcontext.WorkingHours
                .AsNoTracking()
                .FirstOrDefaultAsync(wh => wh.CyberClubId == cyberClubId
                    && wh.DayOfWeek == dayOfWeek)
                        ?? throw new Exception($"Working hours of Cyber club with Id: {cyberClubId} for day {dayOfWeek} not found");

        }

        public async Task UpdateWorkingHours(
            Guid whId,
            CustomDayOfWeek? newDayOfWeek,
            TimeOnly? newStartHour,
            TimeOnly? newEndHour,
            bool? newIsOpen)
         {
            await _dbcontext.WorkingHours
                .Where(wh => wh.Id == whId)
                .ExecuteUpdateAsync(ub =>
                {
                    if (newDayOfWeek is not null)
                    {
                        ub.SetProperty(wh => wh.DayOfWeek, newDayOfWeek);
                    }
                    if (newStartHour is not null)
                    {
                        ub.SetProperty(wh => wh.StartHour, newStartHour);
                    }
                    if (newEndHour is not null)
                    {
                        ub.SetProperty(wh => wh.EndHour, newEndHour);
                    }
                    if (newIsOpen is not null)
                    {
                        ub.SetProperty(wh => wh.IsOpen, newIsOpen);
                    }
                });
         } 
        public async Task DeleteByCCId(Guid ccId)
        {
            await _dbcontext.WorkingHours
                .Where(wh => wh.CyberClubId == ccId)
                .ExecuteDeleteAsync();
        }

        public async Task DeleteByWHId(Guid whId)
        {
            await _dbcontext.WorkingHours
                .Where(wh => wh.Id == whId)
                .ExecuteDeleteAsync();
        }

        
    }
}
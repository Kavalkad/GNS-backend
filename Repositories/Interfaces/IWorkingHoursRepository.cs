using GNS.Data.Entities;
using GNS.Enums;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IWorkingHoursRepository
    {
        Task CreateWorkingHours(WorkingHoursEntity workingHours);
        Task<List<WorkingHoursEntity>> GetWorkingHoursAsync(Guid cyberClubId);
        Task<WorkingHoursEntity> GetDayWorkingHoursAsync(Guid cyberClubId, CustomDayOfWeek dayOfWeek);
        Task UpdateWorkingHours(
            Guid whId,
            CustomDayOfWeek? newDayOfWeek,
            TimeOnly? newStartHour,
            TimeOnly? newEndHour,
            bool? newIsOpen);
        Task DeleteByWHId(Guid whId);
        Task DeleteByCCId(Guid ccId); 
    }
}
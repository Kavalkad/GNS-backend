using GNS.Data.Entities;
using GNS.Enums;

namespace GNS.Data.Repositories.Interfaces
{
    public interface IWorkingHoursRepository
    {
        Task CreateWorkingHours(
            Guid cyberClubId,
            CustomDayOfWeek dayOfWeek,
            TimeOnly startHour,
            TimeOnly endHour,
            bool isOpen);
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
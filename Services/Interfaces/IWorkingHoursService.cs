using GNS.Dto;
using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Enums;

namespace GNS.Services.Interfaces
{
    public interface IWorkingHoursService
    {
        Task AddWorkingHours(AddWorkingHoursRequest request);
        Task<List<WorkingHoursDto>> GetByCCId(Guid cuberClubId);
        Task<WorkingHoursEntity?> GetByDayAndCCId(Guid cyberClubId, CustomDayOfWeek dayOfWeek);
        Task UpdateWorkingHours(UpdateWorkingHoursRequest request);
        Task DeleteByCCId(Guid ccId);
        Task DeleteByWHId(Guid whId);
    }
}
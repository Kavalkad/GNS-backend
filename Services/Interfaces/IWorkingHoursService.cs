using GNS.Dto;
using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Enums;

namespace GNS.Services.Interfaces
{
    public interface IWorkingHoursService
    {
        Task AddWorkingHoursAsync(AddWorkingHoursRequest request, CancellationToken token = default);
        Task<List<WorkingHoursDto>> GetByCyberClubIdAsync(Guid cyberClubId, CancellationToken token = default);
        //Task<WorkingHoursEntity?> GetByDayAndCCId(Guid cyberClubId, CustomDayOfWeek dayOfWeek);
        Task UpdateWorkingHoursAsync(UpdateWorkingHoursRequest request, CancellationToken token = default);
       // Task DeleteByCCId(Guid ccId);
        Task DeleteByWorkingHoursIdAsync(Guid whId, CancellationToken token = default);
    }
}
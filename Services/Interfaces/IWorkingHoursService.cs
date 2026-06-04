using GNS.Dto;
using GNS.Contracts.Requests;
using GNS.Data.Entities;

namespace GNS.Services.Interfaces
{
    public interface IWorkingHoursService
    {
        Task AddWorkingHoursAsync(CreateWorkingHoursRequest request, CancellationToken token = default);
        Task<WorkingHoursEntity> GetByIdAsync(Guid workingHoursId, CancellationToken token = default);
        Task<List<WorkingHoursDto>> GetByCyberClubIdAsync(Guid cyberClubId, CancellationToken token = default);

        Task UpdateWorkingHoursStartHourAsync(
            UpdateWorkingHoursStartHourRequest request,
            CancellationToken token = default
            );
        Task UpdateWorkingHoursEndHourAsync(
            UpdateWorkingHoursEndHourRequest request,
            CancellationToken token = default
            );
        Task UpdateWorkingHoursIsOpenAsync(
            UpdateWorkingHoursIsOpenRequest request,
            CancellationToken token = default
            );

        Task DeleteByWorkingHoursIdAsync(Guid whId, CancellationToken token = default);
    }
}
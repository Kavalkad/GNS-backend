using GNS.Dto;
using GNS.Contracts;
using GNS.Contracts.Requests;

namespace GNS.Services.Interfaces
{
    public interface IWorkingHoursService
    {
        Task CreateWorkingHours(CreateWorkingHoursRequest request);
        Task<List<WorkingHoursDto>> GetWorkingHours(Guid cuberClubId);
        Task UpdateWorkingHours(UpdateWorkingHoursRequest request);
        Task DeleteByCCId(Guid ccId);
        Task DeleteByWHId(Guid whId);
    }
}
using GNS.Contracts.Requests;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface ITimeSlotsService
    {
        Task<IEnumerable<TimeSlotDto>> GetUnAvailableSlotsAsync(
            Guid cyberClubId,
            Guid gamingPlaceId,
            DateOnly date,
            CancellationToken token = default
        );
        Task<IEnumerable<TimeSlotDto>> GetAvailableSlotsAsync(GetAvailableTimeSlotsRequest request);
    }
}
using GNS.Contracts.Requests;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface ITimeSlotsService
    {
        Task<List<TimeSlotDto>> GetAvailableSlotsAsync(
            Guid cyberClubId,
            Guid gamingPlaceId,
            DateOnly date,
            TimeSpan duration,
            CancellationToken token = default
        );
        Task<List<TimeSlotDto>> GetAvailableSlotsAsync(GetAvailableTimeSlotsRequest request);
    }
}
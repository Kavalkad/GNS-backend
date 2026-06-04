using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface ITimeSlotsService
    {
        Task<List<TimeSlotDto>> GetUnAvailableSlotsAsync(
            Guid gamingPlaceId,
            DateOnly date,
            CancellationToken token = default
        );
       
    }
}
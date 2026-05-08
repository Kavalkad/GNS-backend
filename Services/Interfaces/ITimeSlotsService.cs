using GNS.Contracts.Requests;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface ITimeSlotsService
    {
        Task<List<TimeSlotDto>> GetUnAvailableSlotsAsync(
            Guid gamingPlaceId,
            DateTime date,
            CancellationToken token = default
        );
       
    }
}
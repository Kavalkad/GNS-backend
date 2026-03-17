using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Dto;
using GNS.Enums;
using GNS.Extensions;
using GNS.Interfaces;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class TimeSlotsService : ITimeSlotsService
    {
        private readonly IWorkingHoursService _workingHoursService;
        private readonly IOrderService _orderService;


        public TimeSlotsService(
            IWorkingHoursService workingHoursService,
            IOrderService orderService
        )
        {
            _workingHoursService = workingHoursService;
            _orderService = orderService;
        }

        public async Task<IEnumerable<TimeSlotDto>> GetUnAvailableSlotsAsync(
            Guid cyberClubId,
            Guid gamingPlaceId,
            DateOnly date,
            CancellationToken token = default
        )
        {
            var dayOfWeek = date.ParseToCustomDayOfWeek();
            var workingHours = await _workingHoursService.GetByDayAndCCId(cyberClubId, dayOfWeek)
                ?? throw new Exception($"WorkingHours for day: {dayOfWeek} not found.");

            if (!workingHours.IsOpen)
            {
                throw new Exception($"At {date} CyberClub is closed.");
            }
            //var gamingPlace = await _gamingPlacesRepository.GetByIdWithCC(gamingPlaceId);


            var gamingPlaceDateOrders = await _orderService.GetByDateAndGamingPlace(
                date: date,
                gamingPlaceId: gamingPlaceId
            );

            var unavailableTimeSlots = gamingPlaceDateOrders
                .Select(o => new TimeSlotDto(o.StartTime, o.EndTime))
                .OrderBy(ts => ts.StartTime)
                .ToList();

            return unavailableTimeSlots;

        }


        public async Task<IEnumerable<TimeSlotDto>> GetAvailableSlotsAsync(GetAvailableTimeSlotsRequest request)
        {
            return await GetUnAvailableSlotsAsync(
                request.CyberClubId,
                request.GamingPlaceId,
                request.Date);
        }
    }
}
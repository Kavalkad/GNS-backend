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

        public async Task<List<TimeSlotDto>> GetUnAvailableSlotsAsync(
            Guid cyberClubId,
            Guid gamingPlaceId,
            DateTime date,
            CancellationToken token = default
        )
        {
            var dayOfWeek = date.ParseToCustomDayOfWeek();
            var workingHours = await _workingHoursService.GetByCyberClubIdAsync(cyberClubId, token)
                ?? throw new Exception($"WorkingHours for day: {dayOfWeek} not found.");
            var requiredWorkingHours = workingHours.FirstOrDefault(wh => wh.DayOfWeek == dayOfWeek.ToString())
                ?? throw new Exception("TimeSlotService: GetUnavailableSlotsAsync line 37");
            var isOpen = bool.Parse(requiredWorkingHours.IsOpen);

            if (!isOpen)
            {
                Results.Problem($"At the date: {date} cyber club not found.");
                return null;
            }

            var gamingPlaceDateOrders = await _orderService.GetByDateAndGamingPlaceAsync(
                date: date,
                gamingPlaceId: gamingPlaceId,
                token: token
            );

            var unavailableTimeSlots = gamingPlaceDateOrders
                .Select(o => new TimeSlotDto(o.DateTimeStart, o.DateTimeEnd))
                .OrderBy(ts => ts.DateTimeStart)
                .ToList();

            return unavailableTimeSlots;
        }


        public async Task<List<TimeSlotDto>> GetAvailableSlotsAsync(GetAvailableTimeSlotsRequest request)
        {
            return await GetUnAvailableSlotsAsync(
                request.CyberClubId,
                request.GamingPlaceId,
                request.Date);
        }
    }
}
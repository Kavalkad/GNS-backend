using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Dto;
using GNS.Enums;
using GNS.Exceptions;
using GNS.Extensions;
using GNS.Interfaces;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class TimeSlotsService(
        IWorkingHoursService workingHoursService,
        IGamingPlaceService gamingPlaceService,
        IOrderService orderService,
        IMapper mapper
        ) : ITimeSlotsService
    {
        private readonly IWorkingHoursService _workingHoursService = workingHoursService;
        private readonly IGamingPlaceService _gamingPlaceService = gamingPlaceService;
        private readonly IOrderService _orderService = orderService;
        private readonly IMapper _mapper = mapper;


        public async Task<List<TimeSlotDto>> GetUnAvailableSlotsAsync(
            Guid gamingPlaceId,
            DateTime date,
            CancellationToken token = default
        )
        {
            var dayOfWeek = date.Date.ParseToCustomDayOfWeek();

            var gamingPlace = await _gamingPlaceService.GetByIdAsync(gamingPlaceId, token);

            var cyberClubId = gamingPlace.CyberClubId;
            var workingHours = await _workingHoursService.GetByCyberClubIdAsync(cyberClubId, token);

            var stringDayOfWeek = Enum.GetName(dayOfWeek);

            var requiredWorkingHours = workingHours.FirstOrDefault(wh => wh.DayOfWeek == stringDayOfWeek)
                ?? throw new EntityNotFoundException("WorkingHours", $"day of week: {dayOfWeek}");

            if (!requiredWorkingHours.IsOpen)
            {
                return _mapper.MapToTimeSlotDtoList(requiredWorkingHours);
            }

            var gamingPlaceDateOrders = await _orderService.GetByDateAndGamingPlaceAsync(
                date: date,
                gamingPlaceId: gamingPlaceId,
                token: token
            );

            if (gamingPlaceDateOrders.Count == 0)
            {
                return [];
            }


            return _mapper.MapToTimeSlotDtoList(gamingPlaceDateOrders);

        }

    }
}
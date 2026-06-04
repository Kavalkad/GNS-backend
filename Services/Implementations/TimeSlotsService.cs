using GNS.Dto;
using GNS.Exceptions;
using GNS.Extensions;
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
            DateOnly date,
            CancellationToken token = default
        )
        {
            var dateInDateTimeFormat = date.ToDateTime(new TimeOnly()); 
            var dayOfWeek = dateInDateTimeFormat.ParseToCustomDayOfWeek();

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
                date: dateInDateTimeFormat,
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
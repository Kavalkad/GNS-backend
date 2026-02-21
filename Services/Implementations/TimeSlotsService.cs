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
        private readonly IWorkingHoursRepository _workingHoursRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IGamingPlacesRepository _gamingPlacesRepository;

        public TimeSlotsService(
            IWorkingHoursRepository workingHoursRepository,
            IOrdersRepository ordersRepository,
            IGamingPlacesRepository gamingPlacesRepository
        )
        {
            _workingHoursRepository = workingHoursRepository;
            _ordersRepository = ordersRepository;
            _gamingPlacesRepository = gamingPlacesRepository;
        }

        public async Task<List<TimeSlotDto>> GetAvailableSlotsAsync(
            Guid cyberClubId,
            Guid gamingPlaceId,
            DateOnly date,
            TimeSpan duration,
            CancellationToken token = default
        )
        {

            var workingHours = await _workingHoursRepository
                .GetDayWorkingHoursAsync(cyberClubId, date.ParseToCustomDayOfWeek());

            if (!workingHours.IsOpen)
            {
                throw new Exception($"At {date} CyberClub is closed");
            }
            //var gamingPlace = await _gamingPlacesRepository.GetByIdWithCC(gamingPlaceId);


            var dateOrders = await _ordersRepository.GetDateOrdersOfGamingPlace(
                gamingPlaceId,
                date
            );

            var unavailableTimeSlots = dateOrders
                .Select(o => new TimeSlotDto(o.StartTime, o.EndTime))
                .Distinct()
                .OrderBy(ts => ts.StartTime)
                .ToList();

            return CalculateAvailableTimeSlots(
                workingHours.StartHour,
                workingHours.EndHour,
                duration,
                unavailableTimeSlots);

        }



        private List<TimeSlotDto> CalculateAvailableTimeSlots(
            TimeOnly openTime,
            TimeOnly closeTime,
            TimeSpan duration,
            List<TimeSlotDto> unavailableTimeSlots
        )
        {
            var availableSlots = new List<TimeSlotDto>();

            for (
                TimeOnly startTime = openTime,
                endTime = openTime.AddMinutes(duration.Minutes);
                    endTime <= closeTime;
                        startTime.AddMinutes(duration.Minutes),
                        endTime.AddMinutes(duration.Minutes))
            {
                var timeSlot = new TimeSlotDto(startTime, endTime);

                if (!unavailableTimeSlots.Contains(timeSlot))
                {
                    availableSlots.Add(timeSlot);
                }
            }
            return availableSlots;
        }
        public Task<List<TimeSlotDto>> GetAvailableSlotsAsync(GetAvailableTimeSlotsRequest request)
        {
            return GetAvailableSlotsAsync(
                request.CyberClubId,
                request.GamingPlaceId,
                request.Date,
                request.Duration);
        }
    }
}
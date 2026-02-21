using GNS.Dto;
using GNS.Enums;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;

namespace GNS.Services.Implementations
{
    public class WorkingHoursService : IWorkingHoursService
    {
        private readonly IWorkingHoursRepository _workingHoursRepository;
        public WorkingHoursService(IWorkingHoursRepository workingHoursRepository)
        {
            _workingHoursRepository = workingHoursRepository;
        }

        public async Task CreateWorkingHours(CreateWorkingHoursRequest request)
        {
            if (!bool.TryParse(request.IsOpen, out bool _isOpen))
            {
                throw new Exception($"Invalid IsOpen value: {request.IsOpen}");
            }

            if (!Guid.TryParse(request.CyberClubId, out Guid cyberClubId))
            {
                throw new Exception($"Invalid request.CyberClubId value: {request.CyberClubId}");
            }

            await _workingHoursRepository.CreateWorkingHours(
                cyberClubId,
                Enum.Parse<CustomDayOfWeek>(request.DayOfWeek),
                TimeOnly.Parse(request.StartHour),
                TimeOnly.Parse(request.EndHour),
                _isOpen
                );
        }

        public async Task<List<WorkingHoursDto>> GetWorkingHours(Guid cyberClubId)
        {
            var workingHours = await _workingHoursRepository.GetWorkingHoursAsync(cyberClubId);

            return workingHours
                .OrderBy(wh => wh.DayOfWeek)
                .Select(wh => new WorkingHoursDto(wh))
                .ToList();
        }
        public async Task UpdateWorkingHours(UpdateWorkingHoursRequest request)
        {
            CustomDayOfWeek? newDayOfWeek = Enum.Parse<CustomDayOfWeek>(request.NewDayOfWeek);
            TimeOnly? newStartHour = TimeOnly.Parse(request.NewStartHour);
            TimeOnly? newEndHour = TimeOnly.Parse(request.NewEndHour);
            bool? newIsOpen = bool.Parse(request.NewIsOpen);

            await _workingHoursRepository.UpdateWorkingHours(
                request.WorkingHoursId,
                newDayOfWeek,
                newStartHour,
                newEndHour,
                newIsOpen);
        }
        public async Task DeleteByCCId(Guid ccId)
        {
            await _workingHoursRepository.DeleteByCCId(ccId);
        }

        public async Task DeleteByWHId(Guid whId)
        {
            await _workingHoursRepository.DeleteByWHId(whId);
        }


    }
}
using GNS.Dto;
using GNS.Enums;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;
using GNS.Data.Entities;

namespace GNS.Services.Implementations
{
    public class WorkingHoursService : IWorkingHoursService
    {
        private readonly IWorkingHoursRepository _workingHoursRepository;
        private readonly IUnitOfWork _unitOfWork;
        public WorkingHoursService(
            IWorkingHoursRepository workingHoursRepository,
            IUnitOfWork unitOfWork
            )
        {
            _workingHoursRepository = workingHoursRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddWorkingHours(AddWorkingHoursRequest request)
        {
            if (!bool.TryParse(request.IsOpen, out bool _isOpen))
            {
                throw new Exception($"Invalid IsOpen value: {request.IsOpen}");
            }

            if (!Guid.TryParse(request.CyberClubId, out Guid cyberClubId))
            {
                throw new Exception($"Invalid request.CyberClubId value: {request.CyberClubId}");
            }

            var dayOfWeek = Enum.Parse<CustomDayOfWeek>(request.DayOfWeek);

            var workingHours = new WorkingHoursEntity
            {
                CyberClubId = cyberClubId,
                DayOfWeek = dayOfWeek,
                StartHour = TimeOnly.Parse(request.StartHour),
                EndHour = TimeOnly.Parse(request.EndHour),
                IsOpen = _isOpen
            };

            await _workingHoursRepository.CreateWorkingHours(workingHours);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<List<WorkingHoursDto>> GetByCCId(Guid cyberClubId)
        {
            var workingHours = await _workingHoursRepository.GetWorkingHoursAsync(cyberClubId);

            return workingHours
                .OrderBy(wh => wh.DayOfWeek)
                .Select(wh => new WorkingHoursDto(wh))
                .ToList();
        }
        public async Task<WorkingHoursEntity?> GetByDayAndCCId(Guid cyberClubId, CustomDayOfWeek dayOfWeek)
        {
            var workingHours = await _workingHoursRepository.GetWorkingHoursAsync(cyberClubId);

            return workingHours.SingleOrDefault(wh => wh.DayOfWeek == dayOfWeek);
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
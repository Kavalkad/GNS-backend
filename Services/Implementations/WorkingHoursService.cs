using GNS.Dto;
using GNS.Enums;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Exceptions;

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

        public async Task AddWorkingHoursAsync(AddWorkingHoursRequest request, CancellationToken token = default)
        {
            if (!bool.TryParse(request.IsOpen, out bool isOpen))
            {
                throw new IncorrectBoolException(request.IsOpen);
            }


            if (!Guid.TryParse(request.CyberClubId, out Guid cyberClubId))
            {
                throw new IncorrectGuidException(request.CyberClubId);
            }

            if (!Enum.TryParse(request.DayOfWeek, out CustomDayOfWeek dayOfWeek))
            {
                throw new IncorrectDayOfWeekException(request.DayOfWeek);
            }

            if (!TimeOnly.TryParse(request.StartHour, out TimeOnly startHour))
            {
                throw new IncorrectTimeException("Start hour", request.StartHour);
            }

            if (!TimeOnly.TryParse(request.StartHour, out TimeOnly endHour))
            {
                throw new IncorrectTimeException("End hour", request.EndHour);
            }

            var workingHours = new WorkingHoursEntity
            {
                CyberClubId = cyberClubId,
                DayOfWeek = dayOfWeek,
                StartHour = startHour,
                EndHour = endHour,
                IsOpen = isOpen
            };

            await _workingHoursRepository.AddAsync(workingHours, token);
            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task<List<WorkingHoursDto>> GetByCyberClubIdAsync(Guid cyberClubId, CancellationToken token = default)
        {

            var workingHours = await _workingHoursRepository
                .GetByExpressionAsync(wh => wh.CyberClubId == cyberClubId, token);

            return workingHours
                .OrderBy(wh => wh.DayOfWeek)
                .Select(wh => new WorkingHoursDto(wh))
                .ToList();
        }
        /*
        public async Task<WorkingHoursEntity?> GetByDayAndCCId(Guid cyberClubId, CustomDayOfWeek dayOfWeek)
        {
            var workingHours = await _workingHoursRepository.GetWorkingHoursAsync(cyberClubId);

            return workingHours.SingleOrDefault(wh => wh.DayOfWeek == dayOfWeek);
        }
        */

        public async Task UpdateWorkingHoursAsync(UpdateWorkingHoursRequest request, CancellationToken token = default)
        {
            if (!Guid.TryParse(request.WorkingHoursId, out Guid workingHoursId))
            {
                throw new IncorrectGuidException(request.WorkingHoursId);
            }

            var workingHours = await _workingHoursRepository.FindAsync(wh => wh.Id == workingHoursId, token)
                ?? throw new EntityNotFoundException("WorkingHours", request.WorkingHoursId);


            CustomDayOfWeek? newDayOfWeek = Enum.Parse<CustomDayOfWeek>(request.NewDayOfWeek);
            TimeOnly? newStartHour = TimeOnly.Parse(request.NewStartHour);
            TimeOnly? newEndHour = TimeOnly.Parse(request.NewEndHour);
            bool? newIsOpen = bool.Parse(request.NewIsOpen);

            _workingHoursRepository.Update(workingHours);
            await _unitOfWork.SaveChangesAsync(token);
        }
        // Метод не особо нужен потому что EF Core автоматически удалить связанные записи при удалении клуба
       /* public async Task DeleteByCyberClubIdAsync(Guid ccId, CancellationToken token = default)
         {
             await _workingHoursRepository.DeleteByCyberClubId(ccId);
             await _unitOfWork.SaveChangesAsync(token);
         }
 */
        public async Task DeleteByWorkingHoursIdAsync(Guid whId, CancellationToken token = default)
        {
            await _workingHoursRepository.DeleteByIdAsync(whId, token);
            await _unitOfWork.SaveChangesAsync(token);
        }


    }
}
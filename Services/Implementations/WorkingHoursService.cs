using GNS.Dto;
using GNS.Enums;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Exceptions;

namespace GNS.Services.Implementations
{
    public class WorkingHoursService(
        IWorkingHoursRepository workingHoursRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork
            ) : IWorkingHoursService
    {
        private readonly IWorkingHoursRepository _workingHoursRepository = workingHoursRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddWorkingHoursAsync(CreateWorkingHoursRequest request, CancellationToken token = default)
        {
            if (!Enum.TryParse(request.DayOfWeek, out CustomDayOfWeek dayOfWeek))
            {
                throw new IncorrectDayOfWeekException(request.DayOfWeek);
            }

            var workingHours = new WorkingHoursEntity
            {
                CyberClubId = request.CyberClubId,
                DayOfWeek = dayOfWeek,
                StartHour = request.StartHour,
                EndHour = request.EndHour,
                IsOpen = request.IsOpen
            };

            await _workingHoursRepository.AddAsync(workingHours, token);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task<WorkingHoursEntity> GetByIdAsync(Guid workingHoursId, CancellationToken token = default)
        {
            return await _workingHoursRepository.GetByIdAsync(workingHoursId, token)
                ?? throw new EntityNotFoundException("working hours", $"workingHoursId: {workingHoursId}");
        }

        public async Task<List<WorkingHoursDto>> GetByCyberClubIdAsync(Guid cyberClubId, CancellationToken token = default)
        {
            var workingHours = await _workingHoursRepository
                .GetByExpressionAsync(wh => wh.CyberClubId == cyberClubId, token)
                    ?? throw new EntityNotFoundException("working hours", $"cyberClubId: {cyberClubId}");
            return _mapper.MapToWorkingHoursDto(workingHours);
        }


        public async Task UpdateWorkingHoursStartHourAsync(
            UpdateWorkingHoursStartHourRequest request,
            CancellationToken token = default
            )
        {
            var workingHoursId = request.WorkingHoursId;

            var workingHours = await _workingHoursRepository.FindAsync(wh => wh.Id == workingHoursId, token)
                ?? throw new EntityNotFoundException("WorkingHours", workingHoursId.ToString());

            workingHours.StartHour = request.NewStartHour;

            _workingHoursRepository.Update(workingHours);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateWorkingHoursEndHourAsync(
            UpdateWorkingHoursEndHourRequest request,
            CancellationToken token = default
            )
        {
            var workingHoursId = request.WorkingHoursId;

            var workingHours = await _workingHoursRepository.FindAsync(wh => wh.Id == workingHoursId, token)
                ?? throw new EntityNotFoundException("WorkingHours", workingHoursId.ToString());

            workingHours.EndHour = request.NewEndHour;

            _workingHoursRepository.Update(workingHours);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateWorkingHoursIsOpenAsync(
            UpdateWorkingHoursIsOpenRequest request,
            CancellationToken token = default
            )
        {
            var workingHoursId = request.WorkingHoursId;

            var workingHours = await _workingHoursRepository.FindAsync(wh => wh.Id == workingHoursId, token)
                ?? throw new EntityNotFoundException("WorkingHours", workingHoursId.ToString());

            workingHours.IsOpen = request.NewIsOpen;

            _workingHoursRepository.Update(workingHours);
            await _unitOfWork.SaveChangesAsync(token);
        }


        public async Task DeleteByWorkingHoursIdAsync(Guid whId, CancellationToken token = default)
        {
            await _workingHoursRepository.DeleteByIdAsync(whId, token);
            await _unitOfWork.SaveChangesAsync(token);
        }


    }
}
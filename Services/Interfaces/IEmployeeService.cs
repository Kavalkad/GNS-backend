using GNS.Dto;
using GNS.Contracts.Requests;
using GNS.Contracts.Responses;

namespace GNS.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task RegisterAsync(RegisterEmployeeRequest request, CancellationToken token = default);
        Task<LoginEmployeeResponse> LoginAsync(LoginEmployeeRequest request, CancellationToken token = default);

        //Task DeleteById(Guid id);
        Task<List<EmployeeDto>> GetAllAsync(CancellationToken token = default);
        Task<EmployeeDto> GetByNamesAsync(string firstName, string lastName, CancellationToken token = default);
        Task<List<EmployeeDto>> GetWithBonusAsync(CancellationToken token = default);
        Task<List<EmployeeDto>> GetWithPenaltyAsync(CancellationToken token = default);
        Task<List<EmployeeDto>> GetByCyberClubIdAsync(string cyberClubId, CancellationToken token = default);
       // Task<List<EmployeeDto>> GetByCyberClubNameAsync(string cyberClubName, CancellationToken token = default);
        Task UpdateFirstNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        );
        Task UpdateLastNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        );
        Task UpdateRoleNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        );
        Task UpdateCyberClubNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        );
        Task DeleteAsync(DeleteEmployeeRequest request, CancellationToken token = default);
        Task SetZeroBonusesAsync(CancellationToken token = default);
        Task SetZeroPenaltiesAsync(CancellationToken token = default);
        Task GiveBonusAsync(GiveBonusRequest request, CancellationToken token = default);
        Task GivePenaltyAsync(GivePenaltyRequest request, CancellationToken token = default);
    }
}
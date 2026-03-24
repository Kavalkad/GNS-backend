using GNS.Dto;
using GNS.Contracts.Requests;
using GNS.Contracts.Responses;

namespace GNS.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task Register(RegisterEmployeeRequest request);
        Task<LoginEmployeeResponse> Login(LoginEmployeeRequest request);

        //Task DeleteById(Guid id);
        Task<List<EmployeeDto>> GetAll();
        Task<EmployeeDto> GetByNames(string firstName, string lastName);
        Task<List<EmployeeDto>> GetWithBonus();
        Task<List<EmployeeDto>> GetWithPenalty();
        Task<List<EmployeeDto>> GetByCCId(Guid cyberClubId);
        Task<List<EmployeeDto>> GetByCCName(string cyberClubName);
        Task UpdateEmployeeFirstNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        );
        Task UpdateEmployeeLastNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        );
        Task UpdateEmployeeRoleNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        );
        Task UpdateEmployeeCyberClubNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        );
        Task Delete(DeleteEmployeeRequest request);
        Task SetZeroBonuses();
        Task SetZeroPenalties();
        Task GiveBonus(GiveBonusRequest request);
        Task GivePenalty(GivePenaltyRequest request);
    }
}
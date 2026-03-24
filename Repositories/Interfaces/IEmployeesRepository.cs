using GNS.Data.Entities;
using GNS.Enums;


namespace GNS.Data.Repositories.Interfaces
{
    public interface IEmployeesRepository
    {
        Task Register(EmployeeEntity employee);
        Task<EmployeeEntity?> GetById(Guid id);
        Task<EmployeeEntity> GetByEmail(string email);
        Task<EmployeeEntity> GetByNames(string firstName, string LastName);
        Task<List<EmployeeEntity>> GetByCyberClubId(Guid ccId);
        Task<List<EmployeeEntity>> GetByCyberClubName(string cyberClubName);
        Task<List<EmployeeEntity>> GetAllEmployeesAsync();
        Task Update(
            Guid employeeId,
            string? newFirstName = default,
            string? newLastName = default,
            decimal? newSalary = default,
            decimal bonus = 0,
            decimal penalty = 0,
            Role newRole = default,
            Guid newCyberClubId = default,
            CancellationToken token = default
            );
        Task DeleteById(Guid id);
        Task Delete(string firstName, string lastName);

        Task GiveBonus(
            Guid giverId,
            string firstName,
            string lastName,
            decimal bonus
            );
        Task GivePenalty(
            Guid giverId,
            string firstName,
            string lastName,
            decimal bonus
            );
        Task SetZeroBonuses();

        Task SetZeroPenalties();

    }
}
using GNS.Data.Entities;


namespace GNS.Data.Repositories.Interfaces
{
    public interface IEmployeesRepository
    {
        Task Register(
            string email,
            string passwordHash,
            string secretWord,
            string firstName,
            string lastName,
            decimal salary,
            string roleName,
            string cyberClubName
        );
        Task<EmployeeEntity> GetByEmail(string email);
        Task<EmployeeEntity> GetByNames(string firstName, string LastName);
        Task<List<EmployeeEntity>> GetByCyberClubId(Guid ccId);
        Task<List<EmployeeEntity>> GetByCyberClubName(string cyberClubName);
        Task<List<EmployeeEntity>> GetAllEmployeesAsync();
        Task UpdateEmployee(string firstName,
            string lastName,
            string newFirstName,
            string newLastName,
            decimal newSalary,
            string newRoleName,
            string newCyberClubName
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
using System.Security.Claims;
using GNS.Data.Entities;
using GNS.Data.Repositories.Interfaces;
using GNS.Dto;
using GNS.Enums;
using GNS.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GNS.Data.Repositories.Implementations
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly AppDbContext _dbcontext;

        public EmployeesRepository(AppDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task Register(EmployeeEntity employee)
        {

            await _dbcontext.Employees.AddAsync(employee);

        }
        public async Task<EmployeeEntity?> GetById(Guid id)
        {
            return await _dbcontext.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<EmployeeEntity> GetByNames(string firstName, string lastName)
        {
            return await _dbcontext.Employees
                .AsNoTracking()
                .Include(e => e.CyberClub)
                .FirstOrDefaultAsync(e => e.FirstName == firstName && e.LastName == lastName)
                    ?? throw new Exception($"Employee with FirstName {firstName} and LastName {lastName} not found");
        }

        public async Task<EmployeeEntity> GetByEmail(string email)
        {
            return await _dbcontext.Employees
                .AsNoTracking()
                .Include(e => e.CyberClub)
                .FirstOrDefaultAsync(e => e.Email == email)
                    ?? throw new Exception($"Employee with email: {email} not found");
        }

        public async Task<List<EmployeeEntity>> GetByCyberClubId(Guid ccId)
        {
            return await _dbcontext.Employees
                .AsNoTracking()
                .Where(e => e.CyberClubId == ccId)
                .ToListAsync();
        }
        public async Task<List<EmployeeEntity>> GetByCyberClubName(string cyberClubName)
        {
            return await _dbcontext.Employees
                .AsNoTracking()
                .Include(e => e.CyberClub)
                .Where(e => e.CyberClub!.Name == cyberClubName)
                .ToListAsync();
        }
        public async Task<List<EmployeeEntity>> GetAllEmployeesAsync()
        {
            return await _dbcontext.Employees
                .AsNoTracking()
                .Include(e => e.CyberClub)
                .ToListAsync();

        }
        public async Task Update(
            Guid employeeId,
            string? newFirstName = default,
            string? newLastName = default,
            decimal? newSalary = default,
            decimal bonus = 0,
            decimal penalty = 0,
            Role newRole = default,
            Guid newCyberClubId = default,
            CancellationToken token = default
            )
        {

            var employee = await _dbcontext.Employees
                 .FirstOrDefaultAsync(e => e.Id == employeeId, token)
                    ?? throw new Exception($"Employee with id: {employeeId} not found");

            var exceptionMessage = "";

            if (employee.FirstName == newFirstName || string.IsNullOrEmpty(newFirstName))
            {
                exceptionMessage += "Firstname must differ and can't be null or empty|";
            }
            else
            {
                employee.FirstName = newFirstName;
            }

            if (employee.LastName == newLastName || string.IsNullOrEmpty(newLastName))
            {
                exceptionMessage += "Lastname must differ and can't be null or empty|";
            }
            else
            {
            employee.LastName = newLastName;    
            }

            if (newSalary is null || newSalary == employee.Salary)
            {
                exceptionMessage += "New salary must differ and can't be null|";
            }
            else
            {
            employee.Salary = newSalary.Value;    
            }

            if (employee.Role == newRole)
            {
                exceptionMessage += "New role must differ|";
            }
            else
            {
                employee.Role = newRole;
            }

            if (employee.CyberClubId == newCyberClubId)
            {
                exceptionMessage += "new cyber club must differ and can't be 0";
            }
            else
            {
                employee.CyberClubId = newCyberClubId;
            }
            if (bonus > 0)
            {
                employee.Bonus = bonus;
            }

            if (penalty > 0)
            {
                employee.Penalty = penalty;
            }

            if (exceptionMessage.Length != 0)
            {
                throw new Exception(exceptionMessage);
            }
            _dbcontext.Employees.Update(employee);

        }

        public async Task DeleteById(Guid employeeId)
        {
            await _dbcontext.Employees
                .Where(e => e.Id == employeeId)
                .ExecuteDeleteAsync();
        }

        public async Task Delete(string firstName, string lastName)
        {
            await _dbcontext.Employees
                .Where(e => e.FirstName == firstName && e.LastName == lastName)
                .ExecuteDeleteAsync();
        }


        public async Task SetZeroBonuses()
        {

            await _dbcontext.Employees
                .Where(e => e.Bonus != 0)
                .ExecuteUpdateAsync(ub =>
                {
                    ub.SetProperty(e => e.Bonus, 0);
                });

        }
        public async Task SetZeroPenalties()
        {
            await _dbcontext.Employees
                .Where(e => e.Penalty != 0)
                .ExecuteUpdateAsync(ub =>
                {
                    ub.SetProperty(e => e.Penalty, 0);
                });
        }

        public Task GiveBonus(Guid giverId, string firstName, string lastName, decimal bonus)
        {
            throw new NotImplementedException();
        }

        public Task GivePenalty(Guid giverId, string firstName, string lastName, decimal bonus)
        {
            throw new NotImplementedException();
        }
    }
}
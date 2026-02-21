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

        public async Task Register(
            string email,
            string hashedPassword,
            string hashedSecretWord,
            string firstName,
            string lastName,
            decimal salary,
            string roleName,
            string cyberClubName)
        {
            var cyberClub = await _dbcontext.CyberClubs
                .FirstOrDefaultAsync(cc => cc.Name == cyberClubName)
                    ?? throw new Exception("CyberClub not found.");

            var employee = new EmployeeEntity
            {
                Email = email,
                HashedPassword = hashedPassword,
                HashedSecretWord = hashedSecretWord,
                FirstName = firstName,
                LastName = lastName,
                Salary = salary,
                Role = Enum.Parse<Role>(roleName),
                CyberClubId = cyberClub.Id
            };

            await _dbcontext.Employees.AddAsync(employee);
            await _dbcontext.SaveChangesAsync();

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
        public async Task UpdateEmployee(
            string firstName,
            string lastName,
            string newFirstName,
            string newLastName,
            decimal newSalary,
            string newRoleName,
            string newCyberClubName)
        {

            var employee = await _dbcontext.Employees
                 .FirstOrDefaultAsync(e => e.FirstName == firstName && e.LastName == lastName)
                    ?? throw new Exception($"Employee {firstName} {lastName} not found");

            if (!(employee.FirstName == newFirstName && string.IsNullOrEmpty(newFirstName)))
            {
                employee.FirstName = newFirstName;
            }
            if (!(employee.LastName == newLastName && string.IsNullOrEmpty(newLastName)))
            {
                employee.LastName = newLastName;
            }
            if (employee.Salary != newSalary && newSalary != 0)
            {
                employee.Salary = newSalary;
            }
            if (!(newRoleName == nameof(employee.Role) && string.IsNullOrEmpty(newRoleName)))
            {
                employee.Role = Enum.Parse<Role>(newRoleName);
            }
            if (!string.IsNullOrEmpty(newCyberClubName))
            {
                var newCyberClub = await _dbcontext.CyberClubs
                .Where(cc => cc.Name != newCyberClubName)
                .FirstOrDefaultAsync(cc => cc.Name == newCyberClubName)
                    ?? throw new Exception($"CybetClub with name {newCyberClubName} not found");
                employee.CyberClub = newCyberClub;
            }
            _dbcontext.Employees.Update(employee);
            await _dbcontext.SaveChangesAsync();
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

        public async Task GiveBonus(
            Guid giverId,
            string firstName,
            string lastName,
            decimal bonus)
        {
            var giver = await _dbcontext.Employees
                .Include(e => e.CyberClub)
                .FirstOrDefaultAsync(e => e.Id == giverId)
                    ?? throw new Exception("Bonus giver not found");

            var getter = await _dbcontext.Employees
                .Include(e => e.CyberClub)
                .FirstOrDefaultAsync(e => e.FirstName == firstName && e.LastName == lastName)
                    ?? throw new Exception("Bonus getter not found");

            if (giver.Id == getter.Id && giver.Role == Role.Owner)
            {
                getter.Bonus += bonus;
                _dbcontext.Employees.Update(getter);
                await _dbcontext.SaveChangesAsync();
                return;
            }

            if (giver.Role <= getter.Role)
            {
                throw new Exception("Roles don't correspond");
            }

            if (giver.CyberClub.Name != getter.CyberClub.Name)
            {
                throw new Exception("You can't give bonus to employee from other CyberClub");
            }

            getter.Bonus += bonus;

            _dbcontext.Employees.Update(getter);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task GivePenalty(
            Guid giverId,
            string firstName,
            string lastName,
            decimal penalty
            )
        {
            var giver = await _dbcontext.Employees
                .Include(e => e.CyberClub)
                .FirstOrDefaultAsync(e => e.Id == giverId)
                    ?? throw new Exception("Penalty giver not found");

            var getter = await _dbcontext.Employees
                .Include(e => e.CyberClub)
                .FirstOrDefaultAsync(e => e.FirstName == firstName && e.LastName == lastName)
                    ?? throw new Exception("Penalty getter not found");
            if (giver.Id == getter.Id && giver.Role == Role.Owner)
            {
                getter.Penalty += penalty;
                _dbcontext.Employees.Update(getter);
                await _dbcontext.SaveChangesAsync();
                return;
            }
            if (giver.Role <= getter.Role)
            {
                throw new Exception("Roles don't corresponds");
            }
            if (giver.CyberClub!.Name != getter.CyberClub!.Name)
            {
                throw new Exception("You can't give bonus to employee from other CyberClub");
            }

            getter.Penalty += penalty;

            _dbcontext.Employees.Update(getter);
            await _dbcontext.SaveChangesAsync();
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

    }
}
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
    public class EmployeesRepository : BaseRepository<EmployeeEntity>, IEmployeesRepository
    {
        public EmployeesRepository(AppDbContext dbContext) : base(dbContext) 
        {
            
        }

    }
}
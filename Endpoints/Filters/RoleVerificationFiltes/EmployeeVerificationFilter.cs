using System.Security.Claims;
using GNS.Data.Repositories.Interfaces;
using GNS.Enums;
using GNS.Services.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class EmployeeVerificationFilter : IEndpointFilter
    {

        private readonly IEmployeesRepository _employeesRepository;
        public EmployeeVerificationFilter(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }
        
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var employeeStringId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")
                    ?? throw new Exception("Failed to read id claim");

            if (Guid.TryParse(employeeStringId.Value, out Guid employeeId))
            {
                return Results.BadRequest("Id has incorrect format");
            }
            var admin = await _employeesRepository.GetById(employeeId);

            if (admin is null)
            {
                return Results.BadRequest("Employee data doesn't exists");
            }
            
            return await next(context);
        }
    }
}
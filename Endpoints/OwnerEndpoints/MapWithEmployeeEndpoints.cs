using GNS.Contracts.Requests;
using GNS.Endpoints.Filters;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.OwnerEndploints
{
    public static partial class OwnersEndpoints
    {
        public static IEndpointRouteBuilder MapWithEmployeeEndpoints(this IEndpointRouteBuilder app)
        {
            var employees = app.MapGroup("employees");

            employees.MapPost("add", AddEmployee)
                .AddEndpointFilter<OwnerAccessToCyberClubFilter>()
                .AddEndpointFilter<BloomFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            var get = employees.MapGroup("get");
                

            get.MapGet("by-ccid", GetEmployeesByCCId)
                .AddEndpointFilter<OwnerAccessToCyberClubFilter>();


            var update = employees.MapGroup("update")
                .AddEndpointFilter<OwnerAccessToEmployeeFilter>()
                .AddEndpointFilter<NameFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            update.MapPut("firstname", UpdateEmployeeFirstName);
            update.MapPut("lastname", UpdateEmployeeLastName);
            update.MapPut("rolename", UpdateEmployeeRoleName);
            update.MapPut("cyberclub-name", UpdateEmployeeCyberClubName);


            employees.MapDelete("delete", DeleteEmployee)
                .AddEndpointFilter<OwnerAccessToEmployeeFilter>();

            return app;
        }
         public static async Task<IResult> AddEmployee(
            [FromBody] RegisterEmployeeRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.RegisterAsync(request);
            return Results.Ok("Employee successfully registered");
        }
        
        public static async Task<IResult> GetEmployeesByCCId(
            Guid cyberClubId,
            IEmployeeService employeeService
            )
        {
            var employees = await employeeService.GetByCyberClubIdAsync(cyberClubId);
            return TypedResults.Ok(employees);
        }



        public static async Task<IResult> UpdateEmployeeFirstName(
            [FromBody] UpdateEmployeeNameRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.UpdateFirstNameAsync(request);

            return Results.Ok($"Employee's firstname successfully changed on {request.Name}");
        }
        public static async Task<IResult> UpdateEmployeeLastName(
            [FromBody] UpdateEmployeeNameRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.UpdateLastNameAsync(request);

            return Results.Ok($"Employee's lastname successfully changed on {request.Name}");
        }
        public static async Task<IResult> UpdateEmployeeRoleName(
            [FromBody] UpdateEmployeeNameRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.UpdateRoleNameAsync(request);

            return Results.Ok($"Employee's role successfully changed on {request.Name}");
        }
        public static async Task<IResult> UpdateEmployeeCyberClubName(
            [FromBody] UpdateEmployeeNameRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.UpdateLastNameAsync(request);

            return Results.Ok($"Employee was successfully moved to cyberclub with name: {request.Name}");
        }
        public static async Task<IResult> DeleteEmployee(
             Guid employeeId,
            IEmployeeService employeeService
            )
        {
            await employeeService.DeleteAsync(employeeId);
            return Results.Ok("Employee successfully deleted");
        }
    }
}
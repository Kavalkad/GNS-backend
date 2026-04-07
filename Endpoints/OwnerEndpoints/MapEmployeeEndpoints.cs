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
                .AddEndpointFilter<BloomFilter>()
                .AddEndpointFilter<FinalValidationFilter>();

            var get = employees.MapGroup("get");
            get.MapGet("by-ccid", GetCyberClubEmployeesByCCId);
            get.MapGet("all", GetAllEmployees);
            get.MapGet("with-bonus", GetEmployeesWithBonus);
            get.MapGet("with-penalty", GetEmployeesWithPenalty);


            var update = employees.MapGroup("update");
            update.MapPut("firstname", UpdateEmployeeFirstName)
                .AddEndpointFilter<VerifyNameFilter>()
                .AddEndpointFilter<FinalValidationFilter>();
            update.MapPut("lastname", UpdateEmployeeLastName)
                .AddEndpointFilter<VerifyNameFilter>()
                .AddEndpointFilter<FinalValidationFilter>();
            update.MapPut("rolename", UpdateEmployeeRoleName)
                .AddEndpointFilter<VerifyNameFilter>()
                .AddEndpointFilter<FinalValidationFilter>();
            update.MapPut("cyberclub-name", UpdateEmployeeCyberClubName)
                .AddEndpointFilter<VerifyNameFilter>()
                .AddEndpointFilter<FinalValidationFilter>();

            employees.MapDelete("delete", DeleteEmployee);

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
        public static async Task<IResult> GetAllEmployees(
            IEmployeeService employeeService
            )
        {
            var employees = await employeeService.GetAllAsync();
            return TypedResults.Ok(employees);
        }
        public static async Task<IResult> GetCyberClubEmployeesByCCId(
            string cyberClubId,
            IEmployeeService employeeService
            )
        {
            var employees = await employeeService.GetByCyberClubIdAsync(cyberClubId);
            return TypedResults.Ok(employees);
        }
        public static async Task<IResult> GetEmployeesWithBonus(
            IEmployeeService service
            )
        {
            var employeesWithBonus = await service.GetWithBonusAsync();
            return TypedResults.Ok(employeesWithBonus);
        }
        public static async Task<IResult> GetEmployeesWithPenalty(
            IEmployeeService service
            )
        {
            var employeesWithPenalty = await service.GetWithPenaltyAsync();
            return TypedResults.Ok(employeesWithPenalty);
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
            [FromBody] DeleteEmployeeRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.DeleteAsync(request);
            return Results.Ok("Employee successfully deleted");
        }
    }
}
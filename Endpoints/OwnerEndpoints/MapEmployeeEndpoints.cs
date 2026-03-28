using GNS.Contracts.Requests;
using GNS.Endpoints.Filters;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.OwnerEndploints
{
    public static partial class OwnersEndpoints
    {
        public static IEndpointRouteBuilder MapEmployeeEndpoints(this IEndpointRouteBuilder owner)
        {
            var employees = owner.MapGroup("employees");
            employees.MapPost("add", AddEmployee)
                .AddEndpointFilter<BloomFilter>()
                .AddEndpointFilter<FinalValidationFilter>();

            var empGet = employees.MapGroup("get");
            empGet.MapGet("by-ccid", GetCyberClubEmployeesByCCId);
            empGet.MapGet("all", GetAllEmployees);
            empGet.MapGet("with-bonus", GetEmployeesWithBonus);
            empGet.MapGet("with-penalty", GetEmployeesWithPenalty);

            var empUpdate = employees.MapGroup("update");

            empUpdate.MapPut("firstname", UpdateEmployeeFirstName)
                .AddEndpointFilter<VerifyNameFilter>()
                .AddEndpointFilter<FinalValidationFilter>();
            empUpdate.MapPut("lastname", UpdateEmployeeLastName)
                .AddEndpointFilter<VerifyNameFilter>()
                .AddEndpointFilter<FinalValidationFilter>();
            empUpdate.MapPut("rolename", UpdateEmployeeRoleName)
                .AddEndpointFilter<VerifyNameFilter>()
                .AddEndpointFilter<FinalValidationFilter>();
            empUpdate.MapPut("cyberclub-name", UpdateEmployeeCyberClubName)
                .AddEndpointFilter<VerifyNameFilter>()
                .AddEndpointFilter<FinalValidationFilter>();

            employees.MapDelete("delete", DeleteEmployee);

            return owner;
        }
         public static async Task<IResult> AddEmployee(
            [FromBody] RegisterEmployeeRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.Register(request);
            return Results.Ok("Employee successfully registered");
        }
        public static async Task<IResult> GetAllEmployees(
            IEmployeeService employeeService
            )
        {
            var employees = await employeeService.GetAll();
            return TypedResults.Ok(employees);
        }
        public static async Task<IResult> GetCyberClubEmployeesByCCId(
            Guid cyberClubId,
            IEmployeeService employeeService
            )
        {
            var employees = await employeeService.GetByCCId(cyberClubId);
            return TypedResults.Ok(employees);
        }
        public static async Task<IResult> GetEmployeesWithBonus(
            IEmployeeService service
            )
        {
            var employeesWithBonus = await service.GetWithBonus();
            return TypedResults.Ok(employeesWithBonus);
        }
        public static async Task<IResult> GetEmployeesWithPenalty(
            IEmployeeService service
            )
        {
            var employeesWithPenalty = await service.GetWithPenalty();
            return TypedResults.Ok(employeesWithPenalty);
        }


        public static async Task<IResult> UpdateEmployeeFirstName(
            [FromBody] UpdateEmployeeNameRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.UpdateEmployeeFirstNameAsync(request);

            return Results.Ok($"Employee's firstname successfully changed on {request.NewNameValue}");
        }
        public static async Task<IResult> UpdateEmployeeLastName(
            [FromBody] UpdateEmployeeNameRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.UpdateEmployeeLastNameAsync(request);

            return Results.Ok($"Employee's lastname successfully changed on {request.NewNameValue}");
        }
        public static async Task<IResult> UpdateEmployeeRoleName(
            [FromBody] UpdateEmployeeNameRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.UpdateEmployeeRoleNameAsync(request);

            return Results.Ok($"Employee's role successfully changed on {request.NewNameValue}");
        }
        public static async Task<IResult> UpdateEmployeeCyberClubName(
            [FromBody] UpdateEmployeeNameRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.UpdateEmployeeLastNameAsync(request);

            return Results.Ok($"Employee was successfully moved to cyberclub with name: {request.NewNameValue}");
        }
        public static async Task<IResult> DeleteEmployee(
            [FromBody] DeleteEmployeeRequest request,
            IEmployeeService employeeService
            )
        {
            await employeeService.Delete(request);
            return Results.Ok("Employee successfully deleted");
        }
    }
}
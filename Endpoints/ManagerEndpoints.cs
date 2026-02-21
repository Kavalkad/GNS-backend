using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using GNS.Endpoints.Filters;
using GNS.Services;
using GNS.Services.Interfaces;
using GNS.Contracts.Requests;

namespace GNS.Endpoints
{
    public static class ManagerEndpoints
    {
        public static RouteGroupBuilder MapManagerEndpoints(this RouteGroupBuilder employee)
        {
            var manager = employee.MapGroup("manager")
                .RequireAuthorization(policy =>
                {
                    policy.RequireClaim(CustomClaims.ManagerClaim.Type, CustomClaims.ManagerClaim.Value);
                });



            manager.MapGet("get-cyberclub-employees", GetCyberClubEmployees);

            manager.MapGet("get-employee-by-names", GetByNames)
                .AddEndpointFilter<NamesFilter>()
                .AddEndpointFilter<FinalValidationFilter>();



            manager.MapPut("give-bonus", GiveBonus)
                .AddEndpointFilter<PersonFilter>()
                .AddEndpointFilter<BonusFilter>()
                .AddEndpointFilter<FinalValidationFilter>();

            manager.MapPut("give-penalty", GivePenalty)
                .AddEndpointFilter<PersonFilter>()
                .AddEndpointFilter<PenaltyFilter>()
                .AddEndpointFilter<FinalValidationFilter>();

            return employee;
        }

        public static async Task<IResult> GetByNames(
            string firstName,
            string lastName,
            IEmployeeService employeeService
        )
        {
            var employee = await employeeService.GetByNames(firstName, lastName);
            return TypedResults.Ok(employee);
        }
        
        public static async Task<IResult> GetCyberClubEmployees(
            Guid cyberClubId,
            IEmployeeService employeeService
        )
        {
            var employees = await employeeService.GetByCCId(cyberClubId);
            return TypedResults.Ok(employees);
        }
        
        
        public static async Task<IResult> GiveBonus(
            [FromBody] GiveBonusRequest request,
            IEmployeeService service
        )
        {
            await service.GiveBonus(request);
            return Results.Ok();
        }

        public static async Task<IResult> GivePenalty(
            [FromBody] GivePenaltyRequest request,
            IEmployeeService service
        )
        {
            await service.GivePenalty(request);
            return Results.Ok();
        }
    }
}

using GNS.Contracts.Requests;
using GNS.Endpoints.Filters;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints
{
    public static class EmployeesEndpoints
    {
        public static IEndpointRouteBuilder MapEmployeeEndpoints(this IEndpointRouteBuilder app)
        {
            var employee = app.MapGroup("employee");
            employee.MapPost("login", Login)
                .AllowAnonymous();
                
            employee.MapAdminEndpoints();
            employee.MapManagerEndpoints();

            return app;
        }
        public static async Task<IResult> Login(
                [FromBody] LoginEmployeeRequest request,
                IEmployeeService employeeService,
                HttpContext context
            )
        {
            var response = await employeeService.LoginAsync(request);
        
            if (context.Request.Cookies.ContainsKey("accessToken"))
            {
                context.Response.Cookies.Delete("accessToken");
            }
            context.Response.Cookies.Append("accessToken", response.AccessToken);

            if (context.Request.Cookies.ContainsKey("refreshToken"))
            {
                context.Response.Cookies.Delete("refreshToken");
            }
            context.Response.Cookies.Append("refreshToken", response.RefreshToken);
            
            return Results.Ok();
        }
    }
}
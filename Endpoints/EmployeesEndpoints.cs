using GNS.Contracts.Requests;
using GNS.Endpoints.Filters;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GNS.Endpoints
{
    public static class EmployeesEndpoints
    {
        public static IEndpointRouteBuilder MapEmployeeEndpoints(this IEndpointRouteBuilder app)
        {
            var employee = app.MapGroup("employee");
            employee.MapPost("login", Login)
                .AllowAnonymous()
                .AddEndpointFilter<EmailFilter>()
                .AddEndpointFilter<PasswordFilter>()
                .AddEndpointFilter<SecretWordFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            employee.MapAdminEndpoints();
            employee.MapManagerEndpoints();

            return app;
        }
        public static async Task<IResult> Login(
            [FromBody] LoginEmployeeRequest request,
            IEmployeeService employeeService,
            ICookieService cookieService,
            HttpContext context
        )
        {
            var response = await employeeService.LoginAsync(request);

            cookieService.AppendCookie("accessToken", response.AccessToken);

            cookieService.AppendCookie("refreshToken", response.RefreshToken);

            return Results.Ok(new
            {
                response.FirstName,
                response.LastName
            });
        }
    }
}
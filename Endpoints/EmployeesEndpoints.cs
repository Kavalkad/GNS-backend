
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
                HttpContext context
            )
        {
            if (context.Items.TryGetValue("ModelState", out object? modelStateObj)
                && modelStateObj is ModelStateDictionary modelState)
            {
                if (!modelState.IsValid)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsJsonAsync(new ValidationProblemDetails(modelState)
                    {
                        Title = "One or more validation errors occurred.",
                        Status = StatusCodes.Status400BadRequest,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                    });


                }

            }
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
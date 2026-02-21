
using GNS.Contracts.Requests;
using GNS.Services.Interfaces;

namespace GNS.Endpoints
{
    public static class EmployeesEndpoints
    {
        public static IEndpointRouteBuilder MapEmployeeEndpoints(this IEndpointRouteBuilder app)
        {
            var employee = app.MapGroup("employee");
            employee.MapPost("login", Login);


            employee.MapAdminEndpoints();

            employee.MapManagerEndpoints();



            return app;
        }
        public static async Task<IResult> Login(
            LoginEmployeeRequest request,
            IEmployeeService service,
            HttpContext context
        )
        {
            var loginResponse = await service.Login(request);

            if (context.Request.Cookies.ContainsKey("mouse"))
            {
                context.Response.Cookies.Delete("mouse");
            }
            context.Response.Cookies.Append("mouse", loginResponse.Token);
            return Results.Ok(loginResponse.Role);
        }
    }
}
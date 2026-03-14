
using GNS.Contracts.Requests;
using GNS.Services.Interfaces;

namespace GNS.Endpoints
{
    public static class EmployeesEndpoints
    {
        public static IEndpointRouteBuilder MapEmployeeEndpoints(this IEndpointRouteBuilder app)
        {
            var employee = app.MapGroup("employee");

            employee.MapAdminEndpoints();
            employee.MapManagerEndpoints();

            return app;
        }

    }
}
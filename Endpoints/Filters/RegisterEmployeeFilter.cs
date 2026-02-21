
using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Data;
using GNS.Enums;
using GNS.Extensions;

namespace GNS.Endpoints.Filters
{
    public class RegisterEmployeeFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next
            )
        {

            var request = context.Arguments
                .OfType<RegisterEmployeeRequest>()
                .FirstOrDefault();
            if (request is null)
            {
                Results.BadRequest();
            }

            var errors = new Dictionary<string, string[]>();

            if (context.HttpContext.Items.TryGetValue("ValidationErrors", out object? _errors))
            {
                errors = _errors as Dictionary<string, string[]>;
            }


           
            if (!request!.Email.IsEmail())
            {
                errors!.Add("email", [$"Incorrect email format: {request!.Email}"]);
            }

            if (request!.Password.IsNotPassword())
            {
                errors!.Add("password", ["Incorrect password format. Password must contain only letters, digits or punctuation symbols"]);
            }

            if (request.Salary <= 0)
            {
                errors!.Add("salary", ["Salary must be greater than 0"]);
            }

            
            if (request.CyberClubName.IsNotName())
            {
                errors!.Add("cyberclubname", [$"CyberClub with name {request.CyberClubName} doesn't exist"]);

            }

            var roles = Enum.GetNames<Role>();
            if (!roles.Contains(request.RoleName))
            {
                errors!.Add("role", [$"RoleName {request.RoleName} canot be applied"]);
            }
            context.HttpContext.Items["ValidationErrors"] = errors;


            return await next(context);
        }
    }
}
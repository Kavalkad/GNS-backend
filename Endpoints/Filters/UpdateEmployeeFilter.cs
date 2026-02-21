using GNS.Contracts.Requests;
using GNS.Enums;
using GNS.Extensions;


namespace GNS.Endpoints.Filters
{
    public class UpdateEmployeeFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next
            )
        {
            var errors = new Dictionary<string, string[]>();

            if (context.HttpContext.Items.TryGetValue("ValidationErrors", out object? _errors))
            {
                errors = _errors as Dictionary<string, string[]>;
            }
            var request = context.Arguments
                .OfType<UpdateEmployeeRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                Results.BadRequest();
            }
            
            if (request!.FirstName.IsNotName() && !string.IsNullOrEmpty(request.FirstName))
            {
                errors!.Add("firstname", [$"FirstName: {request.FirstName} must contain only letters]"]);
            }
            if (request.LastName.IsNotName() && !string.IsNullOrEmpty(request.LastName))
            {
                errors!.Add("lastname", [$"LastName: {request.LastName} must contain only letters"]);
            }

            if (request.NewSalary <= 0 && request.NewSalary is not null)
            {
                errors!.Add("salary", ["New salary must be greater then 0"]);
            }
            if (!Enum.GetNames<Role>().Contains(request.NewRoleName) && !string.IsNullOrEmpty(request.NewRoleName))
            {
                errors!.Add("role",[$"Role: {request.NewRoleName} doesn't exist"]);
            }
            if (request.NewCyberClubName.IsNotName() && !string.IsNullOrEmpty(request.NewCyberClubName))
            {
                errors!.Add("cyberclub name", ["New cyber club name must contain only letters"]);
            }
            
            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }

   
}
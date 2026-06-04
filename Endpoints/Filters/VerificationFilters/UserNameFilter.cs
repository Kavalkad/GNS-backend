using GNS.Contracts.Requests.Interfaces;
using GNS.Extensions;


namespace GNS.Endpoints.Filters
{
    public class UserNameFilter : IEndpointFilter
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
                .OfType<IUserNameRequest>()
                .FirstOrDefault();


            if (request is null)
            {
                return Results.BadRequest();
            }

            var userName = request.UserName;
            
            if (userName.IsNotUserName())
            {
                errors?.Add("UserName", [$"UserName has incorrect format"]);
            }

            if (userName.Length > 25 || userName.Length < 7)
            {
                errors.Add("username length", ["UserName length must be in interval from 7 to 25"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
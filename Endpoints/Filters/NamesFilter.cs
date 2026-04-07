
using GNS.Extensions;

namespace GNS.Endpoints.Filters
{
    public class NamesFilter : IEndpointFilter
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
            var firstName = context.HttpContext.Request.Query["firstName"].ToString();
            var lastName = context.HttpContext.Request.Query["lastName"].ToString();

            if (firstName.IsNotName())
            {
                errors!.Add("firstname", [$"FirstName: {firstName} must contain only letters"]);
            }
            if (lastName.IsNotName())
            {
                errors!.Add("lastname", [$"LastName: {lastName} must contain only letters"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;
            
            return await next(context);
        }
    }
}
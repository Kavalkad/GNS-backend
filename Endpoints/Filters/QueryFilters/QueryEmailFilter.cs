using GNS.Extensions;

namespace GNS.Endpoints.Filters
{
    public class QueryEmailFilter : IEndpointFilter
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

            var email = context.HttpContext.Request.Query["email"].ToString();

            if (!email.IsEmail())
            {
                errors.Add("format", ["email has incorrect format"]);
            }
            if (email.Length < 7)
            {
                errors.Add("email too short", ["email must contain at least 7 symbols"]);
            }
            if (email.Length > 25)
            {
                errors.Add("email too long", ["email can't have more than 25 symbols"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
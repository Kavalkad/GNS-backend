
using GNS.Extensions;
using Microsoft.AspNetCore.Mvc;

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

            var userName = context.HttpContext.Request.Query["userName"].ToString();

            if (userName.Any(c => !char.IsLetterOrDigit(c)))
            {
                errors!.Add("username", [$"Username {userName} has incorrect format"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
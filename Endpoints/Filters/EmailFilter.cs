
using GNS.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.Filters
{
    public class EmailFilter : IEndpointFilter
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
                errors!.Add("email", [$"Email: {email} has incorrect format"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}

using GNS.Contracts.Requests.Interfaces;
using GNS.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.Filters
{
    public class NameFilter : IEndpointFilter
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
                .OfType<INameRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                return Results.BadRequest("failed to read name from request");
            }
            var name = request.Name;
            if (name.IsNotName())
            {
                errors!.Add("new name", ["Name must contain only letters"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
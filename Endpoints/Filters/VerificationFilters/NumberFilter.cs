using GNS.Contracts.Requests.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class NumberFilter : IEndpointFilter
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
                .OfType<INumberRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                return Results.BadRequest("failed to read name from request");
            }
           
            if (request.Number <= 0)
            {
                errors!.Add("number", ["Number must greater than 0"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
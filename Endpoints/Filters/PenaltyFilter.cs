using GNS.Contracts;
using GNS.Extensions;
using GNS.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class PenaltyFilter : IEndpointFilter
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
                .OfType<IPenaltyRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                Results.BadRequest();
            }

            if (request!.Penalty >= 0)
            {
                errors!.Add("penalty", ["Penalty must be less then 0"]);
            }
            
            context.HttpContext.Items["ValidationErrors"] = errors;
            return await next(context);
        }
    }
}



using GNS.Contracts.Requests.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class BonusFilter : IEndpointFilter
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
                .OfType<IBonusRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                Results.BadRequest("failed to read bonus from request");
            }
            
            if (request!.Bonus <= 0)
            {
                errors!.Add("bonus", ["Bonus must be greater then 0"]);
            }
            
            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
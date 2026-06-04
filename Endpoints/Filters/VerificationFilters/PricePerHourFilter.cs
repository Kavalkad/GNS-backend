using GNS.Contracts.Requests.Interfaces;


namespace GNS.Endpoints.Filters
{
    public class PricePerHourFilter : IEndpointFilter
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
                .OfType<IPricePerHourRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                Results.BadRequest("failed to read price per hour from request");
            }

            if (request!.PricePerHour <= 0)
            {
                errors!.Add("priceperhour", ["PricePerHour can't be less then 0"]);
            }
            
            context.HttpContext.Items["ValidationErrors"] = errors;
            return await next(context);
        }
    }
}
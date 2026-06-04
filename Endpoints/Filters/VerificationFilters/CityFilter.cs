using GNS.Contracts.Requests.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class CityFilter : IEndpointFilter
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
                .OfType<ICityRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                return Results.BadRequest("failed to read city from request");
            }

            if (request.City.Any(c => !char.IsLetter(c)))
            {
                errors!.Add("city", ["City must contain only letters"]);
            }
            
            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
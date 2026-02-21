
namespace GNS.Endpoints.Filters
{
    public class FinalValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            if (context.HttpContext.Items.TryGetValue("ValidationErrors", out object? _errors))
            {
                var errors = _errors as Dictionary<string, string[]>;

                if (errors!.Count > 0)
                {
                    return TypedResults.ValidationProblem(errors);
                    
                }

            }
            return await next(context);

        }
    }
}
namespace GNS.Endpoints.Filters
{
    public class QueryGameTitleFilter : IEndpointFilter
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

            var filter = context.HttpContext.Request.Query["filter"].ToString();

            if (filter.Any(c => !char.IsLetterOrDigit(c) && char.IsWhiteSpace(c)))
            {
                errors?.Add("title", ["game title must contain only letters, digits or whitespace"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
using GNS.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class PersonFilter : IEndpointFilter
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
                .OfType<IPersonRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                errors!.Add("request", ["Invalid request body"]);
            }

            if (request!.FirstName.Any(c => !char.IsLetter(c)))
            {
                errors!.Add("firstname", [$"FirstName: {request.FirstName} must contain only letters"]);
            }
            if (request.LastName.Any(c => !char.IsLetter(c)))
            {
                errors!.Add("lastname", [$"LastName: {request.LastName} must contain only letters"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
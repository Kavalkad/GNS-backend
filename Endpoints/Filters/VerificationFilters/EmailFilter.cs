using GNS.Contracts.Requests.Interfaces;
using GNS.Extensions;


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

            var request = context.Arguments
                .OfType<IEmailRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                return Results.BadRequest("failed to read email from request");
            }

            var email = request.Email;
            if (!email.IsEmail())
            {
                errors?.Add("email", [$"email has incorrect format"]);
            }

             
            if (email.Length < 7 || email.Length > 25)
            {
                errors.Add("email lenth", ["email must contain from 7 to 25 symbols"]);
            }
           

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
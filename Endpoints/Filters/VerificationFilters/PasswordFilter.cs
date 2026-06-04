using GNS.Contracts.Requests.Interfaces;
using GNS.Extensions;

namespace GNS.Endpoints.Filters
{
    public class PasswordFilter : IEndpointFilter
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
                .OfType<IPasswordRequest>()
                .FirstOrDefault();


            if (request is null)
            {
                return Results.BadRequest("failed to read password from request");
            }

            var password = request.Password;
            if (password.IsNotPassword())
            {
                errors?.Add("password", [$"Password must contain only letters, digits and punctuation symbols"]);
            }
            if (password.Length > 25 || password.Length < 7)
            {
                errors.Add("password length", ["password length must be in interval from 7 to 25"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
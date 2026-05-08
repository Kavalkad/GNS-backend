using GNS.Contracts;
using GNS.Contracts.Requests.Interfaces;
using GNS.Extensions;
using GNS.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class SuperSecretWordFilter : IEndpointFilter
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
                .OfType<ISuperSecretWordRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                Results.BadRequest("failed to read secret word");
            }
            var superSecretWord = request.SuperSecretWord;
            if (superSecretWord.Length < 8 || superSecretWord.Length > 25)
            {
                errors!.Add("supersecret word length", ["supersecret word must contain from 8 to 25 symbols"]);
            }

            if (superSecretWord.Any(c => !char.IsLetter(c)))
            {
                errors!.Add("supersecret word", ["supersecret word must contain only letters"]);
            }
            
            context.HttpContext.Items["ValidationErrors"] = errors;
            return await next(context);
        }
    }
}
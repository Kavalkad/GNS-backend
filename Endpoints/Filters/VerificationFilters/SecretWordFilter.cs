using GNS.Contracts;
using GNS.Contracts.Requests.Interfaces;
using GNS.Extensions;
using GNS.Interfaces;

namespace GNS.Endpoints.Filters
{
    public class SecretWordFilter : IEndpointFilter
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
                .OfType<ISecretWordRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                Results.BadRequest("failed to read secret word from request");
            }
            var secretWord = request.SecretWord;
            if (secretWord.Length < 8 || secretWord.Length > 25)
            {
                errors!.Add("secretword length", ["secretword must contain from 8 to 25 symbols"]);
            }

            if (secretWord.Any(c => !char.IsLetter(c)))
            {
                errors!.Add("secretword", ["secretword must contain only letters"]);
            }
            
            context.HttpContext.Items["ValidationErrors"] = errors;
            return await next(context);
        }
    }
}
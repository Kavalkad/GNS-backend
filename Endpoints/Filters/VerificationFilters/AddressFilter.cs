using GNS.Contracts.Requests.Interfaces;
using GNS.Extensions;


namespace GNS.Endpoints.Filters
{
    public class AddressFilter : IEndpointFilter
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
                .OfType<IAddressRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                return Results.BadRequest("failed to read address from request");
            }

            var address = request.Address;
            if (address.IsNotAddress())
            {
                errors!.Add("address", ["address must contain only letters, digits, whitespace or \".\"."]);
            }
            if (address.Length > 64)
            {
                errors.Add("address length", ["address's length must be less then 64"]);
            }
            
            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}

using GNS.Contracts.Requests.Interfaces;
using GNS.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.Filters
{
    public class VerifyAddressFilter : IEndpointFilter
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
                .FirstOrDefault()
                ?? throw new Exception("Invalid request body");

            if (!request.Address.IsAddress())
            {
                errors!.Add("address", ["address must contain only letters, digits, whitespace or \".\"."]);
            }
            
            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
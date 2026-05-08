
using System.Runtime.InteropServices;
using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Enums;
using GNS.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.Filters
{
    public class QueryCityFilter : IEndpointFilter
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

            var city = context.HttpContext.Request.Query["city"].ToString();

            if (city.Any(c => !char.IsLetter(c)))
            {
                errors.Add("city", ["city's name must contain only letters"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
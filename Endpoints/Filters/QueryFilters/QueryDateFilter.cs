
using System.Runtime.InteropServices;
using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Enums;
using GNS.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.Filters
{
    public class QueryDateFilter : IEndpointFilter
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

            var queryDate = context.HttpContext.Request.Query["date"].ToString();
            if (!DateTime.TryParse(queryDate, out DateTime date))
            {
                errors.Add("query date", ["incorrect DateTime format"]);
            }
            var now = DateTime.Now;
            if (date != new DateTime() && date < now)
            {
                errors.Add("query past time", ["You can't order past times in query"]);
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
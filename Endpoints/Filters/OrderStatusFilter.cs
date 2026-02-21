
using System.Runtime.InteropServices;
using GNS.Contracts;
using GNS.Contracts.Requests;
using GNS.Enums;
using GNS.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints.Filters
{
    public class OrderStatusFilter : IEndpointFilter
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
                .OfType<UpdateOrderStatusRequest>()
                .FirstOrDefault();
            if (request is null)
            {
                errors!.Add("request", ["Invalid request body"]);
            }
            else
            {
                if (!Enum.GetNames<OrderStatus>().Contains(request.NewOrderStatus))
                {
                    errors!.Add("orderstatus", [$"OrderStatus: {request.NewOrderStatus} is not exists"]);
                }
            }

            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
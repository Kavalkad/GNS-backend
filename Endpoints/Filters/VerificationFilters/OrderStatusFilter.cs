using GNS.Contracts.Requests;
using GNS.Enums;

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
                return Results.BadRequest("failed to read orderstatus from request");
            }

            if (!Enum.GetNames<OrderStatus>().Contains(request?.NewOrderStatus))
            {
                errors!.Add("orderstatus", [$"OrderStatus: {request?.NewOrderStatus} doesn't exist"]);
            }


            context.HttpContext.Items["ValidationErrors"] = errors;

            return await next(context);
        }
    }
}
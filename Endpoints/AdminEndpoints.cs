using GNS.Contracts.Requests;
using GNS.Endpoints.Filters;
using GNS.Services;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints
{
    public static class AdminEndpoints
    {
        public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
        {
            var admin = app.MapGroup("admin")
                .RequireAuthorization(policy =>
                {
                    policy.RequireClaim(CustomClaims.AdminClaim.Type, CustomClaims.AdminClaim.Value);
                });

            var orders = admin.MapGroup("orders");
            
            orders.MapGet("get-for-today", GetTodaysOrders);

            orders.MapGet("get-by-user-email", GetUserOrdersByEmail)
                .AddEndpointFilter<EmailFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            orders.MapGet("get-by-username", GetOrdersByUserName)
                .AddEndpointFilter<QueryUserNameFilter>()
                .AddEndpointFilter<TerminalValidationFilter>(); ;

            orders.MapPost("update-status", UpdateOrderStatus)
                .AddEndpointFilter<EmployeeAccessToOrderFilter>()
                .AddEndpointFilter<OrderStatusFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();


            return app;
        }
         
        public static async Task<IResult> GetTodaysOrders(IOrderService service)
        {
            var orders = await service.GetTodaysOrdersAsync();

            return TypedResults.Ok(orders);
        }

        public static async Task<IResult> GetUserOrdersByEmail(
            string email,
            IOrderService service
        )
        {
            var orders = await service.GetByUserEmailAsync(email);
            return TypedResults.Ok(orders);
        }
        public static async Task<IResult> GetOrdersByUserName(
            string userName,
            IOrderService service
        )
        {
            var orders = await service.GetByUserNameAsync(userName);
            return TypedResults.Ok(orders);
        }

        public static async Task<IResult> UpdateOrderStatus(
            [FromBody] UpdateOrderStatusRequest request,
            IOrderService service
        )
        {
            await service.UpdateOrderStatusAsync(request.OrderId, request.NewOrderStatus);

            return Results.Ok($"Order status is successfully changed on {request.NewOrderStatus}");
        }

    }
}
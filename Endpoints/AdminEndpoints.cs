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
            orders.MapGet("throwException", ThrowException);
            orders.MapGet("get-by-user-email", GetUserOrdersByEmail)
                .AddEndpointFilter<EmailFilter>()
                .AddEndpointFilter<FinalValidationFilter>();

            orders.MapGet("get-by-username", GetOrdersByUserName)
                .AddEndpointFilter<UserNameFilter>()
                .AddEndpointFilter<FinalValidationFilter>(); ;

            orders.MapPost("update-status", UpdateOrderStatus)
                .AddEndpointFilter<OrderStatusFilter>()
                .AddEndpointFilter<FinalValidationFilter>();


            return app;
        }
         
        public static Task<IResult> ThrowException()
        {
            throw new Exception("Exception thrown");

        }
        public static async Task<IResult> GetTodaysOrders(
            IOrderService service
        )
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
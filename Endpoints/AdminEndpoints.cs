using GNS.Endpoints.Filters;
using GNS.Services;
using GNS.Services.Interfaces;

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

            admin.MapGet("get-orders-for-today", GetTodaysOrders);
            admin.MapGet("throwException", ThrowException);
            admin.MapGet("get-by-user-email", GetUserOrdersByEmail)
                .AddEndpointFilter<EmailFilter>()
                .AddEndpointFilter<FinalValidationFilter>();

            admin.MapGet("get-by-username", GetOrdersByUserName)
                .AddEndpointFilter<UserNameFilter>()
                .AddEndpointFilter<FinalValidationFilter>(); ;

            admin.MapPost("update-order-status", UpdateOrderStatus)
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
            var orders = await service.GetTodaysOrders();

            return TypedResults.Ok(orders);
        }

        public static async Task<IResult> GetUserOrdersByEmail(
            string email,
            IOrderService service
        )
        {
            var orders = await service.GetByUserEmail(email);
            return TypedResults.Ok(orders);
        }
        public static async Task<IResult> GetOrdersByUserName(
            string userName,
            IOrderService service
        )
        {
            var orders = await service.GetByUserName(userName);
            return TypedResults.Ok(orders);
        }

        public static async Task<IResult> UpdateOrderStatus(
            Guid orderId,
            string statusName,
            IOrderService service
        )
        {
            await service.UpdateOrderStatus(orderId, statusName);

            return Results.Ok($"Order status is successfully changed on {statusName}");
        }

    }
}
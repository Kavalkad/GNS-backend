using GNS.Contracts.Requests;
using GNS.Endpoints.Filters;
using GNS.Services;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GNS.Endpoints
{
    public static class UsersEndpoints
    {
        public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("login", Login);

            app.MapPost("user-register", Register)
                .AddEndpointFilter<BloomFilter>()
                .AddEndpointFilter<FinalValidationFilter>();

            var user = app.MapGroup("user")
                .RequireAuthorization(policy =>
                {
                    policy.RequireClaim(CustomClaims.UserClaim.Type, CustomClaims.UserClaim.Value);
                }); ;


            user.MapGet("get-all-clubs", GetAllClubs);

            user.MapGet("get-by-city", GetClubsByCity);
            user.MapPost("create-order", CreateOrder);

            user.MapGet("get-active-orders", GetActiveOrders);
            user.MapGet("get-time-slots", GetAwailableTimeSlots);
            user.MapGet("get-games-by-flter", GetGamesByFilter);
            user.MapDelete("delete-user", DeleteUser);
            return app;
        }
        public static async Task<IResult> Register(
            [FromBody] RegisterUserRequest request,
            IUserService userService
            )
        {
            await userService.Register(request);
            return Results.Ok();
        }

        public static async Task<IResult> Login(
            [FromBody] LoginUserRequest request,
            IUserService userService,
            HttpContext context
        )
        {
            var token = await userService.Login(request);
            
            if (context.Request.Cookies.ContainsKey("mouse"))
            {
                context.Response.Cookies.Delete("mouse");
            }
            context.Response.Cookies.Append("mouse", token);
            return Results.Ok();
        }
        public static async Task<IResult> GetAwailableTimeSlots(
            [FromQuery] GetAvailableTimeSlotsRequest request,
            ITimeSlotsService service
        )
        {
            var timeSlots = await service.GetAvailableSlotsAsync(request);

            return TypedResults.Ok(timeSlots);
        }
        public static async Task<IResult> GetAllClubs(
           ICyberClubService cyberClubService
           )
        {
            var cyberClubs = await cyberClubService.GetAllClubs();
            return TypedResults.Ok(cyberClubs);
        }
        public static async Task<IResult> GetClubsByCity(
            string city,
            ICyberClubService cyberClubService)
        {
            var cityClubs = await cyberClubService.GetByCity(city);
            return TypedResults.Ok(cityClubs);
        }
        public static async Task<IResult> CreateOrder(
            [FromBody] CreateOrderRequest request,
            IOrderService service
        )
        {
            await service.CreateOrder(request);
            return Results.Ok();
        }


        public static async Task<IResult> GetActiveOrders(
            IOrderService service
        )
        {
            var activeOrders = await service.GetActiveOrders();

            return TypedResults.Ok(activeOrders);
        }
        public static async Task<IResult> GetGamesByFilter(
            string filter,
            IGameService gameService
        )
        {
            var games = await gameService.GetByFilter(filter);

            return TypedResults.Ok(games);
        }
        public static async Task<IResult> DeleteUser(IUserService service)
        {
            await service.DeleteUser();
            return Results.Ok();
        }
    }
}
using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Endpoints.Filters;
using GNS.Enums;
using GNS.Extensions;
using GNS.Services;
using GNS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GNS.Endpoints
{
    public static class UsersEndpoints
    {
        public static object MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            var user = app.MapGroup("user")
                .RequireAuthorization(policy =>
                {
                    policy.RequireClaim(CustomClaims.UserClaim.Type, CustomClaims.UserClaim.Value);

                });

            user.MapPost("login", Login)
                .AllowAnonymous();



            user.MapPost("register", Register)
                .AllowAnonymous()
                .AddEndpointFilter<BloomFilter>()
                .AddEndpointFilter<FinalValidationFilter>();

            user.MapGet("refresh", Refresh)
                .AllowAnonymous();

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
            var response = await userService.Login(request);
            
            if (response.Role != Role.User)
            {
                return Results.Unauthorized();
            }

            context.Response.Cookies.Append("accessToken", response.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });

            context.Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });
            return Results.Ok();
        }
        public static async Task<IResult> Refresh(
            IAuthService service,
            IRefreshTokensRepository refreshTokensRepository,
            HttpContext context
            )
        {
            if (!context.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Results.BadRequest("Invalid cookies");
            }

            if (!Guid.TryParse(refreshToken, out Guid refreshTokenValue))
            {
                return Results.BadRequest("Invalid refreshToken value");
            }

            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim is null)
            {
                Console.WriteLine("Cannot find Id claim in token");
                Results.Problem("userIdClaim is null");
            }
            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Results.Problem("userIdClaim.Value has incorrect format");
            }
            var verificationResponse = await service.VerifyRefreshToken(refreshToken, userId);

            if (!verificationResponse.IsValid)
            {
                return Results.Unauthorized();
                throw new SecurityTokenExpiredException("Токен протух");
            }

            var accessToken = await service.GetNewAcessToken(userId);
            context.Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });

            context.Response.Cookies.Append("refreshToken", verificationResponse.NewRefreshToken.Token.ToString(), new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });
            
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
            await service.CreateOrderAsync(request);
            return Results.Ok();
        }


        public static async Task<IResult> GetActiveOrders(
            IOrderService service
        )
        {
            var activeOrders = await service.GetActiveOrdersAsync();

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
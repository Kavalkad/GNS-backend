using GNS.Contracts.Requests;
using GNS.Data.Repositories.Interfaces;
using GNS.Endpoints.Filters;
using GNS.Enums;
using GNS.Exceptions;
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

            user.MapPost("register", Register)
                            .AllowAnonymous()
                            .AddEndpointFilter<EmailFilter>()
                            .AddEndpointFilter<PasswordFilter>()
                            .AddEndpointFilter<UserNameFilter>()
                            .AddEndpointFilter<TerminalValidationFilter>()
                            .AddEndpointFilter<BloomFilter>()
                            .AddEndpointFilter<TerminalValidationFilter>();

            user.MapPost("login", Login)
                .AllowAnonymous()
                .AddEndpointFilter<EmailFilter>()
                .AddEndpointFilter<PasswordFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            user.MapGet("refresh", Refresh)
                .AllowAnonymous();

            user.MapPost("logout", Logout);
            var get = user.MapGroup("get");

            get.MapGet("all-clubs", GetAllClubs);

            get.MapGet("clubs-by-city", GetClubsByCity)
                .AddEndpointFilter<QueryCityFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            get.MapGet("clubs-wh", GetCCWorkingHours);
            get.MapGet("cc-gamingplaces", GetCCGamingPlaces);

            get.MapGet("active-orders", GetActiveOrders);
            get.MapGet("unavailable-time-slots", GetUnAwailableTimeSlots);
            get.MapGet("games-by-flter", GetGamesByFilter)
                .AddEndpointFilter<QueryGameTitleFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            user.MapPost("create-order", CreateOrder)
                .AddEndpointFilter<TimeSpanFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            user.MapDelete("delete-user", DeleteUser);

            return app;
        }
        public static async Task<IResult> Register(
            [FromBody] RegisterUserRequest request,
            IUserService userService
            )
        {
            await userService.RegisterAsync(request);

            return Results.Ok();
        }

        public static async Task<IResult> Login(
            [FromBody] LoginUserRequest request,
            IUserService userService,
            ICookieService cookieService
            )
        {
            var response = await userService.LoginAsync(request);

            if (response.Role != Role.User)
            {
                return Results.Unauthorized();
            }

            cookieService.AppendCookie("accessToken", response.AccessToken);
            cookieService.AppendCookie("refreshToken", response.RefreshToken);
            
            return Results.Ok();
        }
        public static IResult Logout(ICookieService service)
        {
            service.DeleteCookie("accessToken");
            service.DeleteCookie("refreshToken");

            return Results.Ok();
        }
        public static async Task<IResult> Refresh(
            IAuthService authService,
            ICookieService cookieService,
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
                return Results.Problem("Cannot find id claim");
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Results.ValidationProblem(errors: new Dictionary<string, string[]>
                {
                    ["claim"] = ["id claim must have Guid format"]
                });

            }

            var verificationResponse = await authService.VerifyRefreshTokenAsync(refreshToken, userId);

            if (!verificationResponse.IsValid)
            {
                return Results.Unauthorized();
            }

            var accessToken = await authService.GetNewAcessTokenAsync(userId);

            cookieService.AppendCookie("accessToken", accessToken);

            cookieService.AppendCookie("refreshToken", verificationResponse.NewRefreshToken.Token.ToString());

            return Results.Ok();
        }
        public static async Task<IResult> GetCCWorkingHours(
            Guid cyberClubId,
            IWorkingHoursService service
            )
        {
            var workingHours = await service.GetByCyberClubIdAsync(cyberClubId);

            return TypedResults.Ok(workingHours);
        }
        public static async Task<IResult> GetUnAwailableTimeSlots(
            Guid gamingPlaceId,
            DateTime date,
            ITimeSlotsService service
            )
        {

            var timeSlotsDto = await service.GetUnAvailableSlotsAsync(gamingPlaceId, date);

            return TypedResults.Ok(timeSlotsDto);

        }

        public static async Task<IResult> GetAllClubs(ICyberClubService cyberClubService)
        {
            var cyberClubsDto = await cyberClubService.GetAllClubsAsync();

            return TypedResults.Ok(cyberClubsDto);
        }

        public static async Task<IResult> GetClubsByCity(
            string city,
            ICyberClubService cyberClubService
            )
        {

            var cityClubsDto = await cyberClubService.GetByCityAsync(city);

            return TypedResults.Ok(cityClubsDto);

        }
        public static async Task<IResult> GetCCGamingPlaces(
            Guid cyberClubId,
            IGamingPlaceService service
            )
        {
            var gamingPlaces = await service.GetCCGamingPlacesAsync(cyberClubId);

            return TypedResults.Ok(gamingPlaces);
        }
        public static async Task<IResult> CreateOrder(
            [FromBody] CreateOrderRequest request,
            IOrderService service
            )
        {
            var order = await service.CreateOrderAsync(request);

            return Results.Ok(order);
        }


        public static async Task<IResult> GetActiveOrders(IOrderService service)
        {
            var activeOrders = await service.GetActiveOrdersAsync();

            return TypedResults.Ok(activeOrders);
        }

        public static async Task<IResult> GetGamesByFilter(
            string filter,
            IGameService gameService
            )
        {
            var games = await gameService.GetByTitleFilterAsync(filter);

            return TypedResults.Ok(games);
        }
        public static async Task<IResult> DeleteUser(
            IUserService service,
            ICookieService cookieService)
        {
            await service.DeleteUserAsync();

            cookieService.DeleteCookie("accessToken");
            cookieService.DeleteCookie("refreshToken");
            
            return Results.Ok();
        }
    }
}
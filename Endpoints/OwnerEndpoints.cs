using GNS.Services;
using Microsoft.AspNetCore.Mvc;
using GNS.Endpoints.Filters;
using GNS.Contracts.Requests;
using GNS.Services.Interfaces;
using GNS.Endpoints.OwnerEndploints;

namespace GNS.Endpoints
{
    public static class OwnerEndpoints
    {
        public static IEndpointRouteBuilder MapOwnerEndpoints(this IEndpointRouteBuilder app)
        {
            var owner = app.MapGroup("owner")
                .RequireAuthorization(policy =>
                    {
                        policy.RequireClaim(CustomClaims.OwnerClaim.Type, CustomClaims.OwnerClaim.Value);
                    });

            owner.MapPost("register", RegisterOwner)
                .AllowAnonymous()
                .AddEndpointFilter<EmailFilter>()
                .AddEndpointFilter<PasswordFilter>()
                .AddEndpointFilter<UserNameFilter>()
                .AddEndpointFilter<SuperSecretWordFilter>()
                .AddEndpointFilter<TerminalValidationFilter>()
                .AddEndpointFilter<BloomFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();

            owner.MapPost("login", Login)
                .AllowAnonymous()
                .AddEndpointFilter<EmailFilter>()
                .AddEndpointFilter<PasswordFilter>()
                .AddEndpointFilter<SuperSecretWordFilter>()
                .AddEndpointFilter<TerminalValidationFilter>();


            owner.MapWithWorkingHoursEndpoints();
            owner.MapWithCyberClubEndpoints();
            owner.MapWithGamingPlaceEndpoints();
            owner.MapWithGamesEndpoints();
            owner.MapWithEmployeeEndpoints();


            return app;
        }

        public static async Task<IResult> RegisterOwner(
            RegisterOwnerRequest request,
            IOwnerService service
            )
        {
            await service.RegisterOwnerAsync(request);
            return Results.Ok();
        }
        public static async Task<IResult> Login(
            [FromBody] LoginOwnerRequest request,
            IOwnerService service,
            HttpContext context
            )
        {
            var response = await service.LoginAsync(request);

            if (context.Request.Cookies.ContainsKey("accessToken"))
            {
                context.Response.Cookies.Delete("accessToken");
            }
            context.Response.Cookies.Append("accessToken", response.AccessToken);

            if (context.Request.Cookies.ContainsKey("refreshToken"))
            {
                context.Response.Cookies.Delete("refreshToken");
            }
            context.Response.Cookies.Append("refreshToken", response.RefreshToken);
            
            return TypedResults.Ok(new
            {
                response.Email,
                response.UserName,
                response.TaxIdentificationNumber,
                Role = Enum.GetName(response.Role)               
            });
        }
    }
}
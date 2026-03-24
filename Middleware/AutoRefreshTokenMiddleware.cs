
using System.Security.Claims;
using GNS.Data.Repositories.Interfaces;
using GNS.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace GNS.Middleware
{
    public class AutoRefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public AutoRefreshTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IAuthService authService
            )
        {


            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {

                foreach (var c in context.User.Claims)
                {
                    Console.WriteLine(c.Type + " _---_ " + c.Value);
                }

                var refreshSuccess = await TryRefreshTokenAsync(context, authService);

                if (refreshSuccess)
                {
                    Console.WriteLine("Попали в блок:if (refreshSuccess)");
                    context.Response.Clear();
                    await _next(context);
                }
                else
                {
                    Results.Unauthorized();
                }

            }
        }

        private async Task<bool> TryRefreshTokenAsync(
            HttpContext httpContext,
            IAuthService authService
        )
        {
            Console.WriteLine("Начали метод Task<bool> TryRefreshTokenAsync");
            if (!httpContext.Request.Cookies.TryGetValue("refreshToken", out string? refreshToken))
            {
                Console.WriteLine("refreshToken is not found");
                return false;
            }

            if (string.IsNullOrEmpty(refreshToken))
            {
                Console.WriteLine("refreshToken cannot be null");
                return false;
            }

            if (!Guid.TryParse(refreshToken, out Guid guidRefreshToken))
            {
                Console.WriteLine("RefreshToken has incorrect format");
                return false;
            }

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim is null)
            {
                Console.WriteLine("Cannot find Id claim in token");
                return false;
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                Console.WriteLine("IdClaim's value gas incorrect format");
                return false;
            }
            Console.WriteLine("Прошли проверОчки");

            var verificationResponse = await authService.VerifyRefreshToken(refreshToken, userId);
            if (!verificationResponse.IsValid)
            {
                Console.WriteLine("RefreshToken is invalid");
                return false;
            }
            var newAccessToken = await authService.GetNewAcessToken(userId);

            if (httpContext.Request.Cookies.ContainsKey("accessToken"))
            {
                httpContext.Response.Cookies.Delete("accessToken");
            };
           
            httpContext.Response.Cookies.Append("accessToken", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            httpContext.Request.Headers.Authorization = $"Bearer {newAccessToken}";
            httpContext.Response.Cookies.Append("refreshToken", verificationResponse.NewRefreshToken.Token.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return true;
        }
    }
}
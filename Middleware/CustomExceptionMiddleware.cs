using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace GNS.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Results.InternalServerError(ex.Message);
                return;
            }

            
        }
    }
}
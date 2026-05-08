using GNS.Endpoints.Filters;
using GNS.Middleware;
using GNS.Services.Implementations;
using GNS.Services.Interfaces;

namespace GNS.Extensions
{
    public static class CustomMiddlewares
    {
        public static WebApplication UseMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            return app;
        }
    }
}
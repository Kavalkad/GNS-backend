using GNS.Endpoints.Filters;
using GNS.Services.Implementations;
using GNS.Services.Interfaces;

namespace GNS.Extensions
{
    public static class AddMiddleware
    {
        public static WebApplication UseMiddlewares(this WebApplication app)
        {
          //  app.UseMiddleware<AutoRefreshTokenMiddleware>();

            return app;
        }
    }
}
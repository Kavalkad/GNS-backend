using GNS.Middleware;

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
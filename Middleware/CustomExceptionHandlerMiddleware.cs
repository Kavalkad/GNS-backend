using System.ComponentModel.DataAnnotations;
using GNS.Exceptions;
using Microsoft.AspNetCore.Authentication;

namespace GNS.Middleware
{
    public class CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (EntityNotFoundException e)
            {
                // context.Response.StatusCode = StatusCodes.Status204NoContent;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "No content",
                    message = e.Message
                };
                await context.Response.WriteAsJsonAsync(response);

            }
            catch (IncorrectFormatException e)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Bad request",
                    message = e.Message
                };
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (ValidationException e)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Bad request",
                    message = e.Message
                };
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (AuthenticationFailureException e)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Authentication failed",
                    message = e.Message
                };
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (BadHttpRequestException e)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "bad request",
                    message = e.Message
                };
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (AccessViolationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            /*
            catch (Exception e)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Internal error",
                    message = "Oops, something goes wrong. " + e.Message
                };
                await context.Response.WriteAsJsonAsync(response);
            }
            */
        }
    }
}
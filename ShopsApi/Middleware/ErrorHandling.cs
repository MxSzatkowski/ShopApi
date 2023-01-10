using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopsApi.Exceptions;
using System;
using System.Threading.Tasks;

namespace ShopsApi.Middleware
{
    public class ErrorHandling : IMiddleware
    {
        private readonly ILogger<ErrorHandling> _logger;

        public ErrorHandling(ILogger<ErrorHandling> logger)
        {
            _logger = logger;   
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
               await next.Invoke(context);
            }
            catch (ForbidException forbid)
            {
                context.Response.StatusCode = 403;
            }
            catch(NotFound notFound)
            {
                context.Response.StatusCode = 404;
               await context.Response.WriteAsync(notFound.Message);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");

            }
        }
    }
}

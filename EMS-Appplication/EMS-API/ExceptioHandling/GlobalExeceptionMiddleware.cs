using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EMS_API.ExceptioHandling
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalExeceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExeceptionMiddleware> _logger;
        public GlobalExeceptionMiddleware(RequestDelegate next, ILogger<GlobalExeceptionMiddleware> _logger)
        {
            _next = next;
            this._logger = _logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                var statusCode = ex switch
                {
                    ArgumentException => StatusCodes.Status400BadRequest,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };
                _logger.LogError(ex, "Unhandled exception occurred");
                httpContext.Response.StatusCode = statusCode;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
                {
                    Status = statusCode,
                    Title = statusCode switch
                    {
                        StatusCodes.Status400BadRequest => "Bad request",
                        StatusCodes.Status401Unauthorized => "UnAuthorize Request",
                        StatusCodes.Status404NotFound => "Not Found",
                        _ => "Error"
                    },
                    Detail = ex.Message,
                    Instance = httpContext.Request.Path
                });
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GlobalExeceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExeceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExeceptionMiddleware>();
        }
    }
}

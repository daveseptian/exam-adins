using System.Net;

namespace CutiApp.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
                RequestDelegate next,
                ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An unhandled exception occurred. Method: {Method}, Path: {Path}",
                    ctx.Request.Method,
                    ctx.Request.Path
                );

                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //500 Internal Server Error
                await ctx.Response.WriteAsJsonAsync(new
                {
                    status = "error",
                    message = "A server error"
                });
            }

        }
    }
}

using System.Net;
using System.Text;

namespace AzureB2CTester.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                byte[] data = Encoding.UTF8.GetBytes(ex.Message);
                context.Response.ContentType = "text/plain";
                await context.Response.Body.WriteAsync(data, 0, data.Length);
            }
        }
    }
}

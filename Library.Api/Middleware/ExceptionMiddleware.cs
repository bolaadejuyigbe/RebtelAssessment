using System.Net;

namespace Library.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new
                {
                    StatusCode = context.Response.StatusCode,
                    ErrorType = ex.GetType().Name,
                    Message = ex.Message,
                    Detail = ex is ArgumentException || ex is ArgumentNullException
                        ? ex.Message
                        : "An unexpected error occurred. Please try again later."
                };

                var errorJson = System.Text.Json.JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(errorJson);
            }
        }
    }
}

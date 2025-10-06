using System.Net;
using System.Text.Json;

namespace FoodOrderApi.Exceptions
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            } catch(AppException e)
            {
                _logger.LogError(e, e.Message);

                HttpResponse response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = e.Code;
                Object data = new { message = e.Message };
                JsonSerializerOptions options = new()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                string responseBody = JsonSerializer.Serialize(data, options);
                await response.WriteAsync(responseBody);
            } catch(Exception e)
            {
                _logger.LogError(e, e.Message);

                HttpResponse response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status500InternalServerError;
                Object data = new { message = e.Message };
                JsonSerializerOptions options = new()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                string responseBody = JsonSerializer.Serialize(data, options);
                await response.WriteAsync(responseBody);
            }
        }
    }
}

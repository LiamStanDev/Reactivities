using System.Net;
using System.Text.Json;
using Application.Core;

namespace API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(
        RequestDelegate next, // receive a HttpContext return a Task. Use to pass to the next middleware.
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env
    )
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            // the context type "application/json" is the api controller used for response.
            // text/plain：純文本格式。
            // application/xml：XML 格式。
            // application/json：JSON 格式。
            // application/octet-stream：二進制流格式。
            context.Response.ContentType = "application/json"; // header
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // header
            // ApplicationException is runtime exception is inherited from Exception.
            var response = _env.IsDevelopment()
                ? new AppException(
                    context.Response.StatusCode,
                    ex.Message,
                    ex.StackTrace?.ToString() // mean the satcktrace may not have.
                )
                : new AppException(context.Response.StatusCode, "Internal Server Error");
            // because we outside the controller, so we need to setting option for jsonserializer like it's in controller(optional)
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // because of the tradition json naming is camelCase differ from C#
            };
            string json = JsonSerializer.Serialize(response, options);
            System.Console.WriteLine(json);
            await context.Response.WriteAsync(json); // body
        }
    }
}

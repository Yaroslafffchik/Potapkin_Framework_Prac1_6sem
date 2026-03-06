namespace PotapkinPrac1.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;

        _logger.LogInformation(
            "Входящий запрос: RequestId={RequestId}, Method={Method}, Path={Path}, QueryString={QueryString}",
            requestId,
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString
        );

        await _next(context);

        _logger.LogInformation(
            "Исходящий ответ: RequestId={RequestId}, StatusCode={StatusCode}",
            requestId,
            context.Response.StatusCode
        );
    }
}

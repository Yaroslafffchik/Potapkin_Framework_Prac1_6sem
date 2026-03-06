using System.Diagnostics;

namespace PotapkinPrac1.Middleware;

public class TimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TimingMiddleware> _logger;

    public TimingMiddleware(RequestDelegate next, ILogger<TimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var requestId = context.Items["RequestId"]?.ToString() ?? "unknown";

        _logger.LogInformation(
            "Время выполнения запроса: RequestId={RequestId}, Duration={Duration}ms",
            requestId,
            stopwatch.ElapsedMilliseconds
        );
    }
}

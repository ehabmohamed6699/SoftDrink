namespace Billing.Api.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;
    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger) { _next = next; _logger = logger; }
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue("X-Correlation-Id", out var cid) ? cid.ToString() : Guid.NewGuid().ToString();
        context.Response.Headers["X-Correlation-Id"] = correlationId;
        _logger.LogInformation("Request {Method} {Path} CorrelationId={CorrelationId}", context.Request.Method, context.Request.Path, correlationId);
        await _next(context);
        _logger.LogInformation("Response {StatusCode} CorrelationId={CorrelationId}", context.Response.StatusCode, correlationId);
    }
}
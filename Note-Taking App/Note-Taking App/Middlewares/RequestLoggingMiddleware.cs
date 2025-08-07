using Serilog;
using System.Diagnostics;

namespace Note_Taking_App.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var request = context.Request;
            Log.Information("Handling request: {method} {url}", request.Method, request.Path);

            await _next(context);

            stopwatch.Stop();
            var response = context.Response;
            Log.Information("Handled request: {method} {url} => {statusCode} in {elapsed}ms",
                request.Method,
                request.Path,
                response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}

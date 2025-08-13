using System.Diagnostics;

namespace simpleAPI.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("{Method} {Path}", context.Request.Method, context.Request.Path);
            await _next(context);
            
            sw.Stop();
            _logger.LogInformation("{Status} {Method} {Path} in {Elapsed} ms", 
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path,
                sw.ElapsedMilliseconds);
                

        }
    }
}

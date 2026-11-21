using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesCatalog.Middlewares
{
    public class RequestCounterMW
    {
        private readonly RequestDelegate _next;
        private static int _requestCount = 0;

        public RequestCounterMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var count = Interlocked.Increment(ref _requestCount);
            var logMessage = $"=== REQUEST COUNTER ===\nRequest #{count}: {context.Request.Method} {context.Request.Path}";

            await MoviesCatalog.Presentation.Infrastructure.FileLogger.LogAsync(logMessage);

            await _next(context);
        }
    }
}
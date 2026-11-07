using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MoviesCatalog.Middlewares
{
    public class TimestampMW
    {
        private readonly RequestDelegate _next;

        public TimestampMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestTime = DateTime.UtcNow;
            await _next(context);
            var responseTime = DateTime.UtcNow;

            var duration = responseTime - requestTime;
            var logMessage = $"=== TIMESTAMP ===\nRequest received at {requestTime:O}\nResponse sent at {responseTime:O}\nRequest processed in {duration.TotalMilliseconds} ms\n";

            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
            await File.AppendAllTextAsync(logPath, logMessage + "\n");
        }
    }
}
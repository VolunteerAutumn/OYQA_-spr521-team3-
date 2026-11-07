using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace MoviesCatalog.Middlewares
{
    public class BasicInfoMW
    {
        private readonly RequestDelegate _next;

        public BasicInfoMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;

            await _next(context);

            var statusCode = context.Response.StatusCode;

            var logMessage = $"=== BASIC INFO ===\nHTTP Method: {method}\nURL: {path}\nStatus Code: {statusCode}\n";

            // Append to log file
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
            await File.AppendAllTextAsync(logPath, logMessage + "\n");
        }
    }
}
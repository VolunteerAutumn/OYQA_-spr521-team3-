using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace MoviesCatalog.Middlewares
{
    public class IPMW
    {
        private readonly RequestDelegate _next;

        public IPMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var blockedIps = new List<string> { };
            var clientIp = context.Connection.RemoteIpAddress?.ToString();

            var logMessage = $"=== IP INFO ===\nClient IP: {clientIp}";

            if (clientIp != null && blockedIps.Contains(clientIp))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied.");
                logMessage += " (BLOCKED)";
            }
            else
            {
                await _next(context);
            }

            await MoviesCatalog.Presentation.Infrastructure.FileLogger.LogAsync(logMessage);
        }
    }
}
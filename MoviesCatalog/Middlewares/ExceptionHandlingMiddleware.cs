using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MoviesCatalog.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next, 
            ILogger<ExceptionHandlingMiddleware> logger, 
            IWebHostEnvironment env)
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
                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");
                await HandleExceptionAsync(context, ex, _env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment env)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,          // 404 - Resource not found
                UnauthorizedAccessException => HttpStatusCode.Unauthorized, // 401 - Unauthorized access
                ArgumentException => HttpStatusCode.BadRequest,           // 400 - Invalid arguments provided
                InvalidOperationException => HttpStatusCode.BadRequest,   // 400 - Invalid operation / business rule violation
                _ => HttpStatusCode.InternalServerError                     // 500 - Unhandled server errors
            };

            context.Response.StatusCode = (int)statusCode;

            context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = GetFriendlyMessage(statusCode),

                details = env.IsDevelopment() ? exception.Message : null
            };

            var result = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(result);
        }

        private static string GetFriendlyMessage(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.NotFound => "The requested resource was not found.",
                HttpStatusCode.Unauthorized => "Authentication is required to access this resource.",
                HttpStatusCode.BadRequest => "The request was invalid or cannot be otherwise served.",
                _ => "An internal server error occurred. Please try again later."
            };
        }
    }
}
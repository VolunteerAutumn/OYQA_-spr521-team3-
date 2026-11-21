using Microsoft.Extensions.Logging;
using MoviesCatalog.BLL.Interfaces;

namespace MoviesCatalog.BLL.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string toEmail, string subject, string message)
    {
        _logger.LogInformation($"[Email Simulation] To: {toEmail}, Subject: {subject}, Message: {message}");
        return Task.CompletedTask;
    }
}
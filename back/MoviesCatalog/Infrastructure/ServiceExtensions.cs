using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoviesCatalog.BLL.Interfaces;
using MoviesCatalog.BLL.Services;

namespace MoviesCatalog.Presentation.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
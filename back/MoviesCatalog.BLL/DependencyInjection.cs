using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoviesCatalog.DAL;
using MoviesCatalog.BLL.Mapping;
using MoviesCatalog.BLL.Interfaces;
using MoviesCatalog.BLL.Services;

namespace MoviesCatalog.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBll(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add the DAL
        services.AddDal(configuration);

        // Add AutoMapper
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        // Add Services
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IProducerService, ProducerService>();

        return services;
    }
}

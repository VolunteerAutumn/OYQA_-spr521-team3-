using MoviesCatalog.BLL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoviesCatalog.DAL;

namespace MoviesCatalog.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBll(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Add the DAL
        services.AddDal(configuration);

        // 2. Add AutoMapper
        // services.AddAutoMapper(typeof(Mapping.MappingProfile).Assembly);

        // 3. Add Validators
        // services.AddValidatorsFromAssemblyContaining<...>();

        // 4. Add Services
        // services.AddScoped<Interfaces.IMovieService, Services.MovieService>();
        // services.AddScoped<Interfaces.IGenreService, Services.GenreService>();
        // services.AddScoped<Interfaces.IProducerService, Services.ProducerService>();

        return services;
    }
}
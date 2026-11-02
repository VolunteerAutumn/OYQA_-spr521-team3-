
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoviesCatalog.DAL;
using MoviesCatalog.BLL.Mapping;

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
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MoviesCatalog.BLL.Mapping.MappingProfile>();
        });

        // 3. Add Validators

        // 4. Add Services

        return services;
    }
}

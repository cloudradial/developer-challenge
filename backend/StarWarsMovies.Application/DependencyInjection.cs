using Microsoft.Extensions.DependencyInjection;
using StarWarsMovies.Application.Interfaces;
using StarWarsMovies.Application.Services;

namespace StarWarsMovies.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStarWarsService, StarWarsService>();

        return services;
    }
}

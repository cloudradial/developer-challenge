using Microsoft.Extensions.DependencyInjection;
using StarWarsMovies.Application.Services;
using StarWarsMovies.Infrastructure.ExternalApis;

namespace StarWarsMovies.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<IStarWarsApiClient, StarWarsApiClient>();

        return services;
    }
}

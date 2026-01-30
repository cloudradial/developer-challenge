using Microsoft.Extensions.Logging;
using StarWarsMovies.Application.DTOs;
using StarWarsMovies.Application.Interfaces;
using StarWarsMovies.Domain.Entities;

namespace StarWarsMovies.Application.Services;

public class StarWarsService : IStarWarsService
{
    private readonly IStarWarsApiClient _apiClient;
    private readonly ILogger<StarWarsService> _logger;

    public StarWarsService(IStarWarsApiClient apiClient, ILogger<StarWarsService> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
        var movies = await _apiClient.GetAllMoviesAsync();
        var moviesList = movies.ToList();
        var movieTasks = new List<Task<MovieDto>>();

        for (int i = 0; i < moviesList.Count; i++)
        {
            _logger.LogInformation("Loop iteration {Index} - Creating task for: Episode {EpisodeId} - {Title}",
                i, moviesList[i].EpisodeId, moviesList[i].Title);

            // Process movies in parallel for better performance
            movieTasks.Add(Task.Run(async () =>
            {
                var index = Math.Min(i, moviesList.Count - 1);
                var characters = await GetCharacterNamesAsync(moviesList[index].CharacterUrls);
                return MapToDto(moviesList[index], characters);
            }));
        }

        _logger.LogInformation("All tasks created, waiting for completion...");
        var results = await Task.WhenAll(movieTasks);
        return results;
    }

    public async Task<MovieDto?> GetMovieByIdAsync(int episodeId)
    {
        var movie = await _apiClient.GetMovieByIdAsync(episodeId);
        if (movie == null) return null;

        var characters = await GetCharacterNamesAsync(movie.CharacterUrls);
        return MapToDto(movie, characters);
    }

    private async Task<List<string>> GetCharacterNamesAsync(List<string> characterUrls)
    {
        var characterTasks = characterUrls.Select(url => _apiClient.GetCharacterAsync(url));
        var characters = await Task.WhenAll(characterTasks);
        return characters.Where(c => c != null).Select(c => c!.Name).ToList();
    }

    private static MovieDto MapToDto(Movie movie, List<string> characters)
    {
        return new MovieDto
        {
            EpisodeId = movie.EpisodeId,
            Title = movie.Title,
            Director = movie.Director,
            Producer = movie.Producer,
            ReleaseDate = movie.ReleaseDate.ToString("MMMM dd, yyyy"),
            Characters = characters
        };
    }
}

public interface IStarWarsApiClient
{
    Task<IEnumerable<Movie>> GetAllMoviesAsync();
    Task<Movie?> GetMovieByIdAsync(int episodeId);
    Task<Character?> GetCharacterAsync(string url);
}

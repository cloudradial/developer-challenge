using StarWarsMovies.Application.DTOs;
using StarWarsMovies.Application.Interfaces;
using StarWarsMovies.Domain.Entities;

namespace StarWarsMovies.Application.Services;

public class StarWarsService : IStarWarsService
{
    private readonly IStarWarsApiClient _apiClient;

    public StarWarsService(IStarWarsApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
        var movies = await _apiClient.GetAllMoviesAsync();
        var movieDtos = new List<MovieDto>();

        foreach (var movie in movies)
        {
            var characters = await GetCharacterNamesAsync(movie.CharacterUrls);
            movieDtos.Add(MapToDto(movie, characters));
        }

        return movieDtos;
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

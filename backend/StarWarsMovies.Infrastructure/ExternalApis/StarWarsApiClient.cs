using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StarWarsMovies.Application.Services;
using StarWarsMovies.Domain.Entities;

namespace StarWarsMovies.Infrastructure.ExternalApis;

public class StarWarsApiClient : IStarWarsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<StarWarsApiClient> _logger;
    private const string BaseUrl = "https://swapi.info";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    public StarWarsApiClient(HttpClient httpClient, IMemoryCache cache, ILogger<StarWarsApiClient> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
        _cache = cache;
        _logger = logger;
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        const string cacheKey = "all_movies";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<Movie>? cachedMovies) && cachedMovies != null)
        {
            _logger.LogInformation("Returning cached movies");
            return cachedMovies;
        }

        try
        {
            _logger.LogInformation("Fetching movies from SWAPI");
            var response = await _httpClient.GetAsync("/api/films");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Response content (first 200 chars): {Content}", content.Length > 200 ? content.Substring(0, 200) : content);
            var films = JsonSerializer.Deserialize<List<SwapiFilm>>(content);

            if (films == null || films.Count == 0)
            {
                return Enumerable.Empty<Movie>();
            }

            var movies = films.Select(MapToMovie).ToList();

            _cache.Set(cacheKey, movies, CacheDuration);
            _logger.LogInformation("Cached {Count} movies", movies.Count);

            return movies;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching movies from SWAPI");
            throw;
        }
    }

    public async Task<Movie?> GetMovieByIdAsync(int episodeId)
    {
        var cacheKey = $"movie_{episodeId}";

        if (_cache.TryGetValue(cacheKey, out Movie? cachedMovie) && cachedMovie != null)
        {
            _logger.LogInformation("Returning cached movie {EpisodeId}", episodeId);
            return cachedMovie;
        }

        try
        {
            _logger.LogInformation("Fetching movie {EpisodeId} from SWAPI", episodeId);
            var response = await _httpClient.GetAsync($"/api/films/{episodeId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var film = JsonSerializer.Deserialize<SwapiFilm>(content);

            if (film == null)
            {
                return null;
            }

            var movie = MapToMovie(film);
            _cache.Set(cacheKey, movie, CacheDuration);

            return movie;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching movie {EpisodeId} from SWAPI", episodeId);
            throw;
        }
    }

    public async Task<Character?> GetCharacterAsync(string url)
    {
        var cacheKey = $"character_{url}";

        if (_cache.TryGetValue(cacheKey, out Character? cachedCharacter) && cachedCharacter != null)
        {
            return cachedCharacter;
        }

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var swapiCharacter = JsonSerializer.Deserialize<SwapiCharacter>(content);

            if (swapiCharacter == null)
            {
                return null;
            }

            var character = new Character
            {
                Name = swapiCharacter.Name,
                Url = swapiCharacter.Url
            };

            _cache.Set(cacheKey, character, CacheDuration);

            return character;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching character from {Url}", url);
            return null; // Don't throw for individual character failures
        }
    }

    private static Movie MapToMovie(SwapiFilm film)
    {
        return new Movie
        {
            EpisodeId = film.EpisodeId,
            Title = film.Title,
            OpeningCrawl = film.OpeningCrawl,
            Director = film.Director,
            Producer = film.Producer,
            ReleaseDate = DateTime.Parse(film.ReleaseDate),
            CharacterUrls = film.Characters,
            Url = film.Url
        };
    }
}

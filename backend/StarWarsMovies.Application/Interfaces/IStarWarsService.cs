using StarWarsMovies.Application.DTOs;

namespace StarWarsMovies.Application.Interfaces;

public interface IStarWarsService
{
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
    Task<MovieDto?> GetMovieByIdAsync(int episodeId);
}

using Microsoft.AspNetCore.Mvc;
using StarWarsMovies.Application.Interfaces;

namespace StarWarsMovies.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IStarWarsService _starWarsService;
    private readonly ILogger<MoviesController> _logger;

    public MoviesController(IStarWarsService starWarsService, ILogger<MoviesController> logger)
    {
        _starWarsService = starWarsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMovies()
    {
        try
        {
            var movies = await _starWarsService.GetAllMoviesAsync();
            return Ok(movies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all movies");
            return StatusCode(500, new { error = "An error occurred while retrieving movies" });
        }
    }

    [HttpGet("{episodeId}")]
    public async Task<IActionResult> GetMovieById(int episodeId)
    {
        try
        {
            var movie = await _starWarsService.GetMovieByIdAsync(episodeId);
            if (movie == null)
            {
                return NotFound(new { error = $"Movie with episode ID {episodeId} not found" });
            }

            return Ok(movie);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving movie {EpisodeId}", episodeId);
            return StatusCode(500, new { error = "An error occurred while retrieving the movie" });
        }
    }
}

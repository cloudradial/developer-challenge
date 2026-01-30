namespace StarWarsMovies.Application.DTOs;

public class MovieDto
{
    public int EpisodeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Producer { get; set; } = string.Empty;
    public string ReleaseDate { get; set; } = string.Empty;
    public List<string> Characters { get; set; } = new();
}

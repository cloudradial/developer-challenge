namespace StarWarsMovies.Domain.Entities;

public class Movie
{
    public int EpisodeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string OpeningCrawl { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Producer { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public List<string> CharacterUrls { get; set; } = new();
    public string Url { get; set; } = string.Empty;
}

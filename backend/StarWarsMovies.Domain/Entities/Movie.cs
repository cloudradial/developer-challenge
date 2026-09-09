using StarWarsMovies.Contracts;

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

    public static Movie FromSwapiFilm(SwapiFilm film)
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

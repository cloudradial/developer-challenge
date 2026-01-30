using FluentAssertions;
using Moq;
using StarWarsMovies.Application.Services;
using StarWarsMovies.Domain.Entities;

namespace StarWarsMovies.Tests.Services;

public class StarWarsServiceTests
{
    private readonly Mock<IStarWarsApiClient> _mockApiClient;
    private readonly StarWarsService _service;

    public StarWarsServiceTests()
    {
        _mockApiClient = new Mock<IStarWarsApiClient>();
        _service = new StarWarsService(_mockApiClient.Object);
    }

    [Fact]
    public async Task GetAllMoviesAsync_ShouldReturnMoviesWithCharacters()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie
            {
                EpisodeId = 4,
                Title = "A New Hope",
                Director = "George Lucas",
                Producer = "Gary Kurtz",
                ReleaseDate = new DateTime(1977, 5, 25),
                CharacterUrls = new List<string> { "https://swapi.info/api/people/1", "https://swapi.info/api/people/2" }
            }
        };

        var characters = new List<Character>
        {
            new Character { Name = "Luke Skywalker", Url = "https://swapi.info/api/people/1" },
            new Character { Name = "C-3PO", Url = "https://swapi.info/api/people/2" }
        };

        _mockApiClient.Setup(x => x.GetAllMoviesAsync()).ReturnsAsync(movies);
        _mockApiClient.Setup(x => x.GetCharacterAsync(It.IsAny<string>()))
            .ReturnsAsync((string url) => characters.FirstOrDefault(c => c.Url == url));

        // Act
        var result = await _service.GetAllMoviesAsync();

        // Assert
        var moviesList = result.ToList();
        moviesList.Should().HaveCount(1);
        moviesList[0].Title.Should().Be("A New Hope");
        moviesList[0].Characters.Should().HaveCount(2);
        moviesList[0].Characters.Should().Contain("Luke Skywalker");
        moviesList[0].Characters.Should().Contain("C-3PO");
    }

    [Fact]
    public async Task GetMovieByIdAsync_ShouldReturnMovieWithCharacters()
    {
        // Arrange
        var movie = new Movie
        {
            EpisodeId = 5,
            Title = "The Empire Strikes Back",
            Director = "Irvin Kershner",
            Producer = "Gary Kurtz",
            ReleaseDate = new DateTime(1980, 5, 21),
            CharacterUrls = new List<string> { "https://swapi.info/api/people/1" }
        };

        var character = new Character { Name = "Luke Skywalker", Url = "https://swapi.info/api/people/1" };

        _mockApiClient.Setup(x => x.GetMovieByIdAsync(5)).ReturnsAsync(movie);
        _mockApiClient.Setup(x => x.GetCharacterAsync(It.IsAny<string>())).ReturnsAsync(character);

        // Act
        var result = await _service.GetMovieByIdAsync(5);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("The Empire Strikes Back");
        result.Characters.Should().HaveCount(1);
        result.Characters.Should().Contain("Luke Skywalker");
    }

    [Fact]
    public async Task GetMovieByIdAsync_WhenMovieNotFound_ShouldReturnNull()
    {
        // Arrange
        _mockApiClient.Setup(x => x.GetMovieByIdAsync(999)).ReturnsAsync((Movie?)null);

        // Act
        var result = await _service.GetMovieByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllMoviesAsync_ShouldHandleNullCharacters()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie
            {
                EpisodeId = 4,
                Title = "A New Hope",
                Director = "George Lucas",
                Producer = "Gary Kurtz",
                ReleaseDate = new DateTime(1977, 5, 25),
                CharacterUrls = new List<string> { "https://swapi.info/api/people/999" }
            }
        };

        _mockApiClient.Setup(x => x.GetAllMoviesAsync()).ReturnsAsync(movies);
        _mockApiClient.Setup(x => x.GetCharacterAsync(It.IsAny<string>())).ReturnsAsync((Character?)null);

        // Act
        var result = await _service.GetAllMoviesAsync();

        // Assert
        var moviesList = result.ToList();
        moviesList.Should().HaveCount(1);
        moviesList[0].Characters.Should().BeEmpty();
    }
}

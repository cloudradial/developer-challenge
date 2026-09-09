using StarWarsMovies.Contracts;

namespace StarWarsMovies.Domain.Entities;

public class Character
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public static Character FromSwapiCharacter(SwapiCharacter character)
    {
        return new Character
        {
            Name = character.Name,
            Url = character.Url
        };
    }
}

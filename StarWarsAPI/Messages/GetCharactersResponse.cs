using StarWarsAPI.Dtos;

namespace StarWarsAPI.Messages
{
    public class GetCharactersResponse
    {
        public string count { get; set; }
        public string? next { get; set; }
        public string? previous { get; set; }
        public List<CharacterDto> results { get; set; }
    }
}

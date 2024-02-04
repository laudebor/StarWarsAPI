using StarWarsAPI.Dtos;
using StarWarsAPI.Messages;

namespace StarWarsAPI.Services
{
    public interface ICharacterService
    {
        Task<GetCharactersResponse> GetCharacters(int page);
        Task<CharacterDto> GetCharacterById(int id);
        Task<GetCharactersResponse> GetCharacterByName(string? name, int page);
    }
}

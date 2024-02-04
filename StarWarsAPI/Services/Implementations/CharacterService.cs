using Newtonsoft.Json;
using StarWarsAPI.Dtos;
using StarWarsAPI.Exceptions;
using StarWarsAPI.Messages;

namespace StarWarsAPI.Services.Implementations
{
    public class CharacterService : ICharacterService
    {
        private readonly HttpClient _httpClient;

        public CharacterService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("StarwarsApi");
            _httpClient.BaseAddress = new Uri("https://swapi.dev/api/people/");
        }

        public async Task<GetCharactersResponse> GetCharacters(int page)
        {
            if (page < 0)
            {
                throw new ServiceException($"Page number {page} is not valid");
            }

            var response = await _httpClient.GetAsync("");

            if (page > 0)
            {
                response = await _httpClient.GetAsync($"?page={page}");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalApiException($"Error returned from {_httpClient.BaseAddress}: {response.StatusCode}");
            }

            var js = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetCharactersResponse>(js);

            return result;
        }

        public async Task<CharacterDto> GetCharacterById(int id)
        {

            if (id <= 0)
            {
                throw new ServiceException($"Id {id} is not valid");
            }

            var response = await _httpClient.GetAsync($"{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalApiException($"Error returned from {_httpClient.BaseAddress}: {response.StatusCode}");
            }

            var js = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CharacterDto>(js);
            return result;
        }

        public async Task<GetCharactersResponse> GetCharacterByName(string? name, int page)
        {

            if (page < 0)
            {
                throw new ServiceException($"Page number {page} is not valid");
            }

            var queryParameters = new List<string>();

            if (!string.IsNullOrEmpty(name))
            {
                queryParameters.Add($"search={name}");
            }

            if (page > 0)
            {
                queryParameters.Add($"page={page}");
            }

            var queryString = (queryParameters.Count > 0) ? "?" + string.Join("&", queryParameters) : "";

            var response = await _httpClient.GetAsync(queryString);

            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalApiException($"Error returned from {_httpClient.BaseAddress}: {response.StatusCode}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetCharactersResponse>(jsonResponse);

            return result;

        }
    }
}

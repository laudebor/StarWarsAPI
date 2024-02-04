using Microsoft.AspNetCore.Mvc;
using StarWarsAPI.Dtos;
using StarWarsAPI.Exceptions;
using StarWarsAPI.Messages;
using StarWarsAPI.Services;
using System.Net;

namespace StarWarsAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ICharacterService _characterService;
        private readonly ILogger<CharacterController> _logger;

        public CharacterController(IHttpClientFactory httpClientFactory, ICharacterService characterService, ILogger<CharacterController> logger)
        {
            _httpClient = httpClientFactory.CreateClient("StarwarsApi");
            _httpClient.BaseAddress = new Uri("https://swapi.dev/api/people/");
            _characterService = characterService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the list of Star Wars characters paginated
        /// </summary>
        /// <returns>The list of Star Wars characters</returns>
        /// <response code = "200">Returns the list of characters</response>
        /// <response code = "400">If page number is not valid</response>
        /// <response code = "404">If external Star Wars API throws an exception</response>
        /// <response code = "500">General internal service errors</response>
        /// <param name="page"></param>
        [ProducesResponseType(typeof(GetCharactersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetCharacters(int page)
        {

            try
            {
                var response = await _characterService.GetCharacters(page);

                return new JsonResult(response);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (ExternalApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets a specific Star Wars character based on the id provided
        /// </summary>
        /// <returns>A Star Wars character</returns>
        /// <response code = "200">Returns a character</response>
        /// <response code = "400">If id is not valid</response>
        /// <response code = "404">If external Star Wars API throws an exception</response>
        /// <response code = "500">General internal service errors</response>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacterById(int id)
        {
            try
            {
                var response = await _characterService.GetCharacterById(id);

                return new JsonResult(response);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (ExternalApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of Star Wars characters filtered by name and paginated 
        /// </summary>
        /// <returns>A list of Star Wars characters based on the name</returns>
        /// <response code = "200">Returns the filtered list of characters</response>
        /// <response code = "400">If page number is not valid</response>
        /// <response code = "404">If external Star Wars API throws an exception</response>
        /// <response code = "500">General internal service errors</response>
        /// <param name="name"></param>
        /// <param name="page"></param>
        [ProducesResponseType(typeof(GetCharactersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetCharacterByName(string? name, int page)
        {
            try
            {
                var response = await _characterService.GetCharacterByName(name, page);

                return new JsonResult(response);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (ExternalApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}

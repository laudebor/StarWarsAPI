using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using StarWarsAPI.Dtos;
using StarWarsAPI.Exceptions;
using StarWarsAPI.Messages;
using StarWarsAPI.Services.Implementations;
using System.Net;

namespace StarWarsAPITests
{
    public class CharacterServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();
        private readonly CharacterService _sut;
        private readonly Mock<HttpMessageHandler> _handlerMock = new();

        public CharacterServiceTests()
        {
            _httpClientFactoryMock.Setup(factory => factory.CreateClient("StarwarsApi")).Returns(new HttpClient(_handlerMock.Object));
            _sut = new CharacterService(_httpClientFactoryMock.Object);

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-10)]
        public async Task GetCharacters_When_Page_Number_Is_Invalid_Should_Throw_ServiceException(int pageNumber)
        {
            //Arrange
            int page = pageNumber;

            //Act || Assert
            await Assert.ThrowsAsync<ServiceException>(() => _sut.GetCharacters(page));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetCharacters_When_Page_Valid_Should_Return_Characters(int pageNumber)
        {
            //Arrange
            int page = pageNumber;
            var response = new GetCharactersResponse();
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(response))
            };
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                 ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>()
                 ).ReturnsAsync(mockResponse);

            //Act
            var result = await _sut.GetCharacters(page);

            //Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task GetCharacters_When_Response_Is_Not_Successful_Should_Throw_ExternalApiException(int page)
        {
            //Arrange
            var response = new GetCharactersResponse();
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(mockResponse);

            //Act || Assert
            await Assert.ThrowsAsync<ExternalApiException>(() => _sut.GetCharacters(page));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        public async Task GetCharacterById_When_Id_Is_Invalid_Should_Throw_ServiceException(int id)
        {
            //Act || Assert
            await Assert.ThrowsAsync<ServiceException>(() => _sut.GetCharacterById(id));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task GetCharacterById_When_Id_Is_Valid_Should_Return_Character_Accordingly(int id)
        {
            //Arrange           
            var response = new CharacterDto();
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(response))
            };
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                 ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>()
                 ).ReturnsAsync(mockResponse);

            //Act
            var result = await _sut.GetCharacterById(id);

            //Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(20)]
        public async Task GetCharacterById_When_Response_Is_Not_Successful_Should_Throw_ExternalApiException(int id)
        {
            //Arrange
            var response = new CharacterDto();
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(mockResponse);

            //Act || Assert
            await Assert.ThrowsAsync<ExternalApiException>(() => _sut.GetCharacterById(id));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-10)]
        public async Task GetCharactersByName_When_Page_Number_Is_Invalid_Should_Throw_ServiceException(int pageNumber)
        {
            //Arrange
            int page = pageNumber;

            //Act || Assert
            await Assert.ThrowsAsync<ServiceException>(() => _sut.GetCharacterByName("Luke", page));
        }

        [Fact]
        public async Task GetCharactersByName_When_Page_Valid_Should_Return_Characters()
        {
            //Arrange
            var response = new GetCharactersResponse();
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(response))
            };
            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(mockResponse);

            //Act
            var result = await _sut.GetCharacterByName(It.IsAny<string>(), 0);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCharactersByName_When_Response_Is_Not_Successful_Should_Throw_ExternalApiException()
        {
            //Arrange
            var response = new GetCharactersResponse();
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(mockResponse);

            //Act || Assert
            await Assert.ThrowsAsync<ExternalApiException>(() => _sut.GetCharacterByName(It.IsAny<string>(), 1));
        }
    }
}
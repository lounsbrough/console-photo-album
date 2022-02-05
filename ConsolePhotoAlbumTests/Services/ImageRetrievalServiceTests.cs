namespace ConsolePhotoAlbumTests.Services;

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using ConsolePhotoAlbum.DataTransferObjects;
using ConsolePhotoAlbum.Services;
using ConsolePhotoAlbumTests.Helpers;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

public class ImageRetrievalServiceTests : TestBase
{
    private readonly ImageRetrievalService _subjectUnderTest;
    private readonly int _expectedAlbumId;
    private MockHttpMessageHandler _httpMessageHandlerMock;

    public ImageRetrievalServiceTests()
    {
        _expectedAlbumId = Fixture.Create<int>();

        _httpMessageHandlerMock = new MockHttpMessageHandler
        {
            MockResponse = string.Empty,
            MockStatusCode = HttpStatusCode.OK
        };

        var httpClient = new HttpClient(_httpMessageHandlerMock);

        _subjectUnderTest = new ImageRetrievalService(httpClient);
    }

    public class RetrieveImagesInAlbum : ImageRetrievalServiceTests
    {
        [Fact]
        public async Task GivenAlbumId_WhenRetrievingImagesInAlbum_AndResponseOk_ThenRequestsCorrectEndpoint()
        {
            var expectedEndpoint = $"https://jsonplaceholder.typicode.com/photos?albumId={_expectedAlbumId}";

            await _subjectUnderTest.RetrieveImagesInAlbum(_expectedAlbumId);

            _httpMessageHandlerMock.RecordedRequests.First().RequestUri.Should().Be(expectedEndpoint);
        }

        [Fact]
        public async Task GivenAlbumId_WhenRetrievingImagesInAlbum_AndResponseOk_ThenReturnsAlbumImages()
        {
            var expectedAlbumImages = Fixture.CreateMany<AlbumImage>();

            _httpMessageHandlerMock.MockResponse = JsonConvert.SerializeObject(expectedAlbumImages);

            var actualAlbumImages = await _subjectUnderTest.RetrieveImagesInAlbum(_expectedAlbumId);

            actualAlbumImages.Should().BeEquivalentTo(expectedAlbumImages);
        }

        [Fact]
        public async Task GivenAlbumId_WhenRetrievingImagesInAlbum_AndResponseNotOk_ThenThrowsException()
        {
            _httpMessageHandlerMock.MockStatusCode = HttpStatusCode.ServiceUnavailable;

            Func<Task> function = async () => await _subjectUnderTest.RetrieveImagesInAlbum(_expectedAlbumId);

            await function.Should().ThrowAsync<HttpRequestException>().WithMessage("Unable to retrieve images from api");
        }
    }
}

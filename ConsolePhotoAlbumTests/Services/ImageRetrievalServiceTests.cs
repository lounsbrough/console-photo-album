namespace ConsolePhotoAlbumTests.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using ConsolePhotoAlbum.DataTransferObjects;
using ConsolePhotoAlbum.Services;
using Helpers;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

public class ImageRetrievalServiceTests : TestBase
{
    private readonly ImageRetrievalService _subjectUnderTest;
    private readonly int? _expectedAlbumId;
    private readonly string? _expectedSearchText;
    private MockHttpMessageHandler _httpMessageHandlerMock;

    protected ImageRetrievalServiceTests()
    {
        _expectedAlbumId = Fixture.Create<int>();
        _expectedSearchText = Fixture.Create<string>();

        _httpMessageHandlerMock = new MockHttpMessageHandler
        {
            MockResponse = string.Empty,
            MockStatusCode = HttpStatusCode.OK
        };

        var httpClient = new HttpClient(_httpMessageHandlerMock);

        _subjectUnderTest = new ImageRetrievalService(httpClient);
    }

    public class RetrieveImages : ImageRetrievalServiceTests
    {
        [Fact]
        public async Task GivenNoAlbumId_AndNoSearchText_WhenRetrievingImages_ThenRequestsCorrectEndpoint()
        {
            var expectedEndpoint = "https://jsonplaceholder.typicode.com/photos";

            await _subjectUnderTest.RetrieveImages(null, null);

            _httpMessageHandlerMock.RecordedRequests.First().RequestUri.Should().Be(expectedEndpoint);
        }

        [Fact]
        public async Task GivenAlbumId_WhenRetrievingImages_ThenRequestsCorrectEndpoint()
        {
            var expectedEndpoint = $"https://jsonplaceholder.typicode.com/photos?albumId={_expectedAlbumId}";

            await _subjectUnderTest.RetrieveImages(_expectedAlbumId, null);

            _httpMessageHandlerMock.RecordedRequests.First().RequestUri.Should().Be(expectedEndpoint);
        }

        [Fact]
        public async Task GivenNoSearchTest_WhenRetrievingImages_AndResponseOk_ThenReturnsAllImages()
        {
            var expectedAlbumImages = Fixture.CreateMany<Image>();

            _httpMessageHandlerMock.MockResponse = JsonConvert.SerializeObject(expectedAlbumImages);

            var actualAlbumImages = await _subjectUnderTest.RetrieveImages(_expectedAlbumId, null);

            actualAlbumImages.Should().BeEquivalentTo(expectedAlbumImages);
        }

        [Fact]
        public async Task GivenSearchTest_WhenRetrievingImages_AndResponseOk_ThenReturnsImagesMatchingSearchText()
        {
            var expectedAlbumImages = Fixture.CreateMany<Image>(3).ToList();

            expectedAlbumImages[1].Title = $"abc{_expectedSearchText}def";

            _httpMessageHandlerMock.MockResponse = JsonConvert.SerializeObject(expectedAlbumImages);

            var actualAlbumImages = await _subjectUnderTest.RetrieveImages(_expectedAlbumId, _expectedSearchText);

            actualAlbumImages.Should().BeEquivalentTo(new List<Image>
            {
                expectedAlbumImages[1]
            });
        }

        [Fact]
        public async Task GivenNoSearchResults_WhenRetrievingImages_AndResponseOk_ThenReturnsEmptyList()
        {
            _httpMessageHandlerMock.MockResponse = string.Empty;

            var actualAlbumImages = await _subjectUnderTest.RetrieveImages(_expectedAlbumId, null);

            actualAlbumImages.Should().BeEmpty();
        }

        [Fact]
        public async Task WhenRetrievingImages_AndResponseNotOk_ThenThrowsException()
        {
            _httpMessageHandlerMock.MockStatusCode = HttpStatusCode.ServiceUnavailable;

            Func<Task> function = async () => await _subjectUnderTest.RetrieveImages(null, null);

            await function.Should().ThrowAsync<HttpRequestException>().WithMessage("Unable to retrieve images from api");
        }
    }
}

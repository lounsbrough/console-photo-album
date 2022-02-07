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

public class DataRetrievalServiceTests : TestBase
{
    private readonly DataRetrievalService _subjectUnderTest;
    private readonly int? _expectedAlbumId;
    private readonly string? _expectedSearchText;
    private readonly MockHttpMessageHandler _httpMessageHandlerMock;

    protected DataRetrievalServiceTests()
    {
        _expectedAlbumId = Fixture.Create<int>();
        _expectedSearchText = Fixture.Create<string>();

        _httpMessageHandlerMock = new MockHttpMessageHandler
        {
            MockResponse = string.Empty,
            MockStatusCode = HttpStatusCode.OK
        };

        var httpClient = new HttpClient(_httpMessageHandlerMock);

        _subjectUnderTest = new DataRetrievalService(httpClient);
    }

    public class RetrieveImages : DataRetrievalServiceTests
    {
        [Fact]
        public async Task GivenNoAlbumId_AndNoSearchText_WhenRetrievingImages_ThenRequestsCorrectEndpoint()
        {
            const string expectedEndpoint = "https://jsonplaceholder.typicode.com/photos";

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
        public async Task GivenAlbumId_AndNoSearchText_WhenRetrievingImages_AndResponseOk_ThenReturnsAllImagesInAlbum()
        {
            var expectedImages = Fixture.CreateMany<Image>();

            _httpMessageHandlerMock.MockResponse = JsonConvert.SerializeObject(expectedImages);

            var actualImages = await _subjectUnderTest.RetrieveImages(_expectedAlbumId, null);

            actualImages.Should().BeEquivalentTo(expectedImages);
        }

        [Fact]
        public async Task GivenAlbumId_AndSearchText_WhenRetrievingImages_AndResponseOk_ThenReturnsImagesMatchingSearchText()
        {
            var expectedImages = Fixture.CreateMany<Image>(3).ToList();

            expectedImages[1].Title = $"abc{_expectedSearchText}def";

            _httpMessageHandlerMock.MockResponse = JsonConvert.SerializeObject(expectedImages);

            var actualImages = await _subjectUnderTest.RetrieveImages(_expectedAlbumId, _expectedSearchText);

            actualImages.Should().BeEquivalentTo(new List<Image>
            {
                expectedImages[1]
            });
        }

        [Fact]
        public async Task GivenNoSearchResults_WhenRetrievingImages_AndResponseOk_ThenReturnsEmptyList()
        {
            _httpMessageHandlerMock.MockResponse = string.Empty;

            var actualImages = await _subjectUnderTest.RetrieveImages(_expectedAlbumId, null);

            actualImages.Should().BeEmpty();
        }

        [Fact]
        public async Task WhenRetrievingImages_AndResponseNotOk_ThenThrowsException()
        {
            _httpMessageHandlerMock.MockStatusCode = HttpStatusCode.ServiceUnavailable;

            Func<Task> function = async () => await _subjectUnderTest.RetrieveImages(null, null);

            await function.Should().ThrowAsync<HttpRequestException>().WithMessage("Unable to retrieve images from api");
        }
    }

    public class RetrieveAlbums : DataRetrievalServiceTests
    {
        [Fact]
        public async Task GivenNoSearchText_WhenRetrievingAlbums_ThenRequestsCorrectEndpoint()
        {
            const string expectedEndpoint = "https://jsonplaceholder.typicode.com/albums";

            await _subjectUnderTest.RetrieveAlbums(null);

            _httpMessageHandlerMock.RecordedRequests.First().RequestUri.Should().Be(expectedEndpoint);
        }

        [Fact]
        public async Task GivenNoSearchText_WhenRetrievingAlbums_AndResponseOk_ThenReturnsAllAlbums()
        {
            var expectedAlbums = Fixture.CreateMany<Album>();

            _httpMessageHandlerMock.MockResponse = JsonConvert.SerializeObject(expectedAlbums);

            var actualAlbums = await _subjectUnderTest.RetrieveAlbums(null);

            actualAlbums.Should().BeEquivalentTo(expectedAlbums);
        }

        [Fact]
        public async Task GivenSearchText_WhenRetrievingAlbums_AndResponseOk_ThenReturnsAlbumsMatchingSearchText()
        {
            var expectedAlbums = Fixture.CreateMany<Album>(3).ToList();

            expectedAlbums[1].Title = $"abc{_expectedSearchText}def";

            _httpMessageHandlerMock.MockResponse = JsonConvert.SerializeObject(expectedAlbums);

            var actualAlbums = await _subjectUnderTest.RetrieveAlbums(_expectedSearchText);

            actualAlbums.Should().BeEquivalentTo(new List<Album>
            {
                expectedAlbums[1]
            });
        }

        [Fact]
        public async Task GivenNoSearchResults_WhenRetrievingAlbums_AndResponseOk_ThenReturnsEmptyList()
        {
            _httpMessageHandlerMock.MockResponse = string.Empty;

            var actualAlbums = await _subjectUnderTest.RetrieveAlbums(null);

            actualAlbums.Should().BeEmpty();
        }

        [Fact]
        public async Task WhenRetrievingAlbums_AndResponseNotOk_ThenThrowsException()
        {
            _httpMessageHandlerMock.MockStatusCode = HttpStatusCode.ServiceUnavailable;

            Func<Task> function = async () => await _subjectUnderTest.RetrieveAlbums(null);

            await function.Should().ThrowAsync<HttpRequestException>().WithMessage("Unable to retrieve albums from api");
        }
    }
}

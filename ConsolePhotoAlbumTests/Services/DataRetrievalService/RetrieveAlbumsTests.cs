namespace ConsolePhotoAlbumTests.Services.DataRetrievalService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using ConsolePhotoAlbum.DataTransferObjects;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

public class RetrieveAlbums : DataRetrievalServiceTestBase
{
    [Fact]
    public async Task GivenNoAlbumId_AndNoSearchText_WhenRetrievingAlbums_ThenRequestsCorrectEndpoint()
    {
        const string expectedEndpoint = "https://jsonplaceholder.typicode.com/albums";

        await SubjectUnderTest.RetrieveAlbums(null, null);

        HttpMessageHandlerMock.RecordedRequests.First().RequestUri.Should().Be(expectedEndpoint);
    }

    [Fact]
    public async Task GivenAlbumId_WhenRetrievingAlbums_ThenRequestsCorrectEndpoint()
    {
        var expectedEndpoint = $"https://jsonplaceholder.typicode.com/albums?id={ExpectedAlbumId}";

        await SubjectUnderTest.RetrieveAlbums(ExpectedAlbumId, null);

        HttpMessageHandlerMock.RecordedRequests.First().RequestUri.Should().Be(expectedEndpoint);
    }

    [Fact]
    public async Task GivenAlbumId_AndNoSearchText_WhenRetrievingAlbums_AndResponseOk_ThenReturnsAllAlbumsInAlbum()
    {
        var expectedAlbums = Fixture.CreateMany<Album>();

        HttpMessageHandlerMock.MockResponse = JsonConvert.SerializeObject(expectedAlbums);

        var actualAlbums = await SubjectUnderTest.RetrieveAlbums(ExpectedAlbumId, null);

        actualAlbums.Should().BeEquivalentTo(expectedAlbums);
    }

    [Fact]
    public async Task GivenAlbumId_AndSearchText_WhenRetrievingAlbums_AndResponseOk_ThenReturnsAlbumsMatchingSearchText()
    {
        var expectedAlbums = Fixture.CreateMany<Album>(3).ToList();

        expectedAlbums[1].Title = $"abc{ExpectedSearchText}def";

        HttpMessageHandlerMock.MockResponse = JsonConvert.SerializeObject(expectedAlbums);

        var actualAlbums = await SubjectUnderTest.RetrieveAlbums(ExpectedAlbumId, ExpectedSearchText);

        actualAlbums.Should().BeEquivalentTo(new List<Album>
        {
            expectedAlbums[1]
        });
    }

    [Fact]
    public async Task GivenNoSearchResults_WhenRetrievingAlbums_AndResponseOk_ThenReturnsEmptyList()
    {
        HttpMessageHandlerMock.MockResponse = string.Empty;

        var actualAlbums = await SubjectUnderTest.RetrieveAlbums(ExpectedAlbumId, null);

        actualAlbums.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenRetrievingAlbums_AndResponseNotOk_ThenThrowsException()
    {
        HttpMessageHandlerMock.MockStatusCode = HttpStatusCode.ServiceUnavailable;

        Func<Task> function = async () => await SubjectUnderTest.RetrieveAlbums(null, null);

        await function.Should().ThrowAsync<HttpRequestException>().WithMessage("Unable to retrieve albums from api");
    }
}

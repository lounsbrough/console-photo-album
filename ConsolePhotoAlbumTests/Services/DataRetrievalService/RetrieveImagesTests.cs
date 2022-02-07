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

public class RetrieveImages : DataRetrievalServiceTestBase
{
    [Fact]
    public async Task GivenNoAlbumId_AndNoSearchText_WhenRetrievingImages_ThenRequestsCorrectEndpoint()
    {
        const string expectedEndpoint = "https://jsonplaceholder.typicode.com/photos";

        await SubjectUnderTest.RetrieveImages(null, null);

        HttpMessageHandlerMock.RecordedRequests.First().RequestUri.Should().Be(expectedEndpoint);
    }

    [Fact]
    public async Task GivenAlbumId_WhenRetrievingImages_ThenRequestsCorrectEndpoint()
    {
        var expectedEndpoint = $"https://jsonplaceholder.typicode.com/photos?albumId={ExpectedAlbumId}";

        await SubjectUnderTest.RetrieveImages(ExpectedAlbumId, null);

        HttpMessageHandlerMock.RecordedRequests.First().RequestUri.Should().Be(expectedEndpoint);
    }

    [Fact]
    public async Task GivenAlbumId_AndNoSearchText_WhenRetrievingImages_AndResponseOk_ThenReturnsAllImagesInAlbum()
    {
        var expectedImages = Fixture.CreateMany<Image>();

        HttpMessageHandlerMock.MockResponse = JsonConvert.SerializeObject(expectedImages);

        var actualImages = await SubjectUnderTest.RetrieveImages(ExpectedAlbumId, null);

        actualImages.Should().BeEquivalentTo(expectedImages);
    }

    [Fact]
    public async Task GivenAlbumId_AndSearchText_WhenRetrievingImages_AndResponseOk_ThenReturnsImagesMatchingSearchText()
    {
        var expectedImages = Fixture.CreateMany<Image>(3).ToList();

        expectedImages[1].Title = $"abc{ExpectedSearchText}def";

        HttpMessageHandlerMock.MockResponse = JsonConvert.SerializeObject(expectedImages);

        var actualImages = await SubjectUnderTest.RetrieveImages(ExpectedAlbumId, ExpectedSearchText);

        actualImages.Should().BeEquivalentTo(new List<Image>
        {
            expectedImages[1]
        });
    }

    [Fact]
    public async Task GivenNoSearchResults_WhenRetrievingImages_AndResponseOk_ThenReturnsEmptyList()
    {
        HttpMessageHandlerMock.MockResponse = string.Empty;

        var actualImages = await SubjectUnderTest.RetrieveImages(ExpectedAlbumId, null);

        actualImages.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenRetrievingImages_AndResponseNotOk_ThenThrowsException()
    {
        HttpMessageHandlerMock.MockStatusCode = HttpStatusCode.ServiceUnavailable;

        Func<Task> function = async () => await SubjectUnderTest.RetrieveImages(null, null);

        await function.Should().ThrowAsync<HttpRequestException>().WithMessage("Unable to retrieve images from api");
    }
}

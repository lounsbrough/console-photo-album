namespace ConsolePhotoAlbumTests.Services.DataRetrievalService;

using System.Net;
using System.Net.Http;
using AutoFixture;
using ConsolePhotoAlbum.Services;
using Helpers;

public class DataRetrievalServiceTestBase : TestBase
{
    protected DataRetrievalServiceTestBase()
    {
        ExpectedAlbumId = Fixture.Create<int>();
        ExpectedSearchText = Fixture.Create<string>();

        HttpMessageHandlerMock = new MockHttpMessageHandler
        {
            MockResponse = string.Empty,
            MockStatusCode = HttpStatusCode.OK
        };

        var httpClient = new HttpClient(HttpMessageHandlerMock);

        SubjectUnderTest = new DataRetrievalService(httpClient);
    }

    protected DataRetrievalService SubjectUnderTest { get; }

    protected int? ExpectedAlbumId { get; }

    protected string? ExpectedSearchText { get; }

    protected MockHttpMessageHandler HttpMessageHandlerMock { get; }
}

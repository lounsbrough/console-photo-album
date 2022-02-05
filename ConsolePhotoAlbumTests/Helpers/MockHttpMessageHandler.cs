using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePhotoAlbumTests.Helpers;

public class MockHttpMessageHandler : HttpMessageHandler
{
    public string? MockResponse { get; set; }

    public HttpStatusCode MockStatusCode { get; set; }

    private string? Input { get; set; }

    public List<HttpRequestMessage> RecordedRequests { get; } = new();

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        RecordedRequests.Add(request);

        if (request.Content != null)
        {
            Input = await request.Content.ReadAsStringAsync(cancellationToken);
        }

        return new HttpResponseMessage
        {
            StatusCode = MockStatusCode,
            Content = new StringContent(MockResponse ?? string.Empty)
        };
    }
}

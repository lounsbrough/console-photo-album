using System;
using ConsolePhotoAlbum.DataTransferObjects;
using Newtonsoft.Json;

namespace ConsolePhotoAlbum.Services;

public class ImageRetrievalService : IImageRetrievalService
{
    private readonly HttpClient _httpClient;

    public ImageRetrievalService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<IEnumerable<AlbumImage>> RetrieveImagesInAlbum(int albumId)
    {
        var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/photos?albumId={albumId}");

        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<IEnumerable<AlbumImage>>(responseContent)!;
        }

        throw new HttpRequestException("Unable to retrieve images from api");
    }
}

namespace ConsolePhotoAlbum.Services;

using DataTransferObjects;
using Interfaces;
using Newtonsoft.Json;

public class ImageRetrievalService : IImageRetrievalService
{
    private readonly HttpClient _httpClient;

    public ImageRetrievalService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Image>> RetrieveImages(int? albumId, string? searchText)
    {
        var endpoint = "https://jsonplaceholder.typicode.com/photos";

        if (albumId != null)
        {
            endpoint += $"?albumId={albumId}";
        }

        var response = await _httpClient.GetAsync(endpoint);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Unable to retrieve images from api");
        }

        var retrievedImages = JsonConvert.DeserializeObject<IEnumerable<Image>>(responseContent);

        if (retrievedImages == null)
        {
            return new List<Image>();
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            retrievedImages = retrievedImages
                .Where(image => image.Title != null && image.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase));
        }

        return retrievedImages;
    }
}

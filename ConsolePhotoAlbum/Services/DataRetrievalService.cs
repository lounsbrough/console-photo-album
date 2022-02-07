namespace ConsolePhotoAlbum.Services;

using DataTransferObjects;
using Interfaces;
using Newtonsoft.Json;

public class DataRetrievalService : IDataRetrievalService
{
    private readonly HttpClient _httpClient;

    public DataRetrievalService(HttpClient httpClient)
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

    public async Task<IEnumerable<Album>> RetrieveAlbums(string? searchText)
    {
        const string endpoint = "https://jsonplaceholder.typicode.com/albums";

        var response = await _httpClient.GetAsync(endpoint);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Unable to retrieve albums from api");
        }

        var retrievedAlbums = JsonConvert.DeserializeObject<IEnumerable<Album>>(responseContent);

        if (retrievedAlbums == null)
        {
            return new List<Album>();
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            retrievedAlbums = retrievedAlbums
                .Where(album => album.Title != null && album.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase));
        }

        return retrievedAlbums;
    }
}

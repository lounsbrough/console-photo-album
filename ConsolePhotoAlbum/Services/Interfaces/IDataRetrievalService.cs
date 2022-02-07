namespace ConsolePhotoAlbum.Services.Interfaces;

using DataTransferObjects;

public interface IDataRetrievalService
{
    Task<IEnumerable<Image>> RetrieveImages(int? albumId, string? searchText);

    Task<IEnumerable<Album>> RetrieveAlbums(string? searchText);
}

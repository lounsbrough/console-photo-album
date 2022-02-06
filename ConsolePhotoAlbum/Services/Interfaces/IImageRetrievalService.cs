namespace ConsolePhotoAlbum.Services.Interfaces;

using DataTransferObjects;

public interface IImageRetrievalService
{
    Task<IEnumerable<Image>> RetrieveImages(int? albumId, string? searchText);
}

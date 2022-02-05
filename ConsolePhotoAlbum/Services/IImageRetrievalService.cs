using ConsolePhotoAlbum.DataTransferObjects;

namespace ConsolePhotoAlbum.Services;

public interface IImageRetrievalService
{
    Task<IEnumerable<AlbumImage>> RetrieveImagesInAlbum(int albumId);
}

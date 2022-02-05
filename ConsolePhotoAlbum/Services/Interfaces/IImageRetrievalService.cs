namespace ConsolePhotoAlbum.Services;

using ConsolePhotoAlbum.DataTransferObjects;

public interface IImageRetrievalService
{
    Task<IEnumerable<AlbumImage>> RetrieveImagesInAlbum(int albumId);
}

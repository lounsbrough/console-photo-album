namespace ConsolePhotoAlbum.Services.Interfaces;

using DataTransferObjects;

public interface IUserInterfaceService
{
    ParsedCommandLineArguments? ParseCommandLineArguments(IEnumerable<string> commandLineArguments);

    void ShowUserInstructions();

    void ShowAlbumListing(List<Album> retrievedAlbums);

    void ShowImageListing(List<Image> retrievedImages);

    void ShowNoResultsFoundMessage();

    void ShowUnhandledExceptionMessage();
}

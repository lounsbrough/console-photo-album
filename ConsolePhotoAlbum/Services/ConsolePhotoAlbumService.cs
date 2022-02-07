namespace ConsolePhotoAlbum.Services;

using Enums;
using Interfaces;

public class ConsolePhotoAlbumService : IConsolePhotoAlbumService
{
    private readonly IUserInputService _userInputService;
    private readonly IDataRetrievalService _dataRetrievalService;

    public ConsolePhotoAlbumService(
        IUserInputService userInputService,
        IDataRetrievalService dataRetrievalService)
    {
        _userInputService = userInputService;
        _dataRetrievalService = dataRetrievalService;
    }

    public async Task RunProgram()
    {
        var commandLineArguments = Environment.GetCommandLineArgs();

        var parsedCommandLineArguments = _userInputService.ParseCommandLineArguments(commandLineArguments);

        if (parsedCommandLineArguments == null)
        {
            return;
        }

        parsedCommandLineArguments.Flags.TryGetValue(AvailableFlags.AlbumId, out var albumId);

        parsedCommandLineArguments.Flags.TryGetValue(AvailableFlags.SearchText, out var searchText);

        if (parsedCommandLineArguments.Resource == AvailableResources.Albums)
        {
            var retrievedAlbums = (await _dataRetrievalService.RetrieveAlbums(albumId as int?, searchText as string)).ToList();

            _userInputService.ShowAlbumListing(retrievedAlbums);
        }
        else if (parsedCommandLineArguments.Resource == AvailableResources.Images)
        {
            var retrievedImages = (await _dataRetrievalService.RetrieveImages(albumId as int?, searchText as string)).ToList();

            _userInputService.ShowImageListing(retrievedImages);
        }
    }
}

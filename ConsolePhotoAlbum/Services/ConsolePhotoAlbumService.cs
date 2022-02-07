namespace ConsolePhotoAlbum.Services;

using Enums;
using Interfaces;

public class ConsolePhotoAlbumService : IConsolePhotoAlbumService
{
    private readonly IUserInterfaceService _userInterfaceService;
    private readonly IDataRetrievalService _dataRetrievalService;

    public ConsolePhotoAlbumService(
        IUserInterfaceService userInterfaceService,
        IDataRetrievalService dataRetrievalService)
    {
        _userInterfaceService = userInterfaceService;
        _dataRetrievalService = dataRetrievalService;
    }

    public async Task RunProgram()
    {
        try
        {
            var commandLineArguments = Environment.GetCommandLineArgs();

            var parsedCommandLineArguments = _userInterfaceService.ParseCommandLineArguments(commandLineArguments);

            if (parsedCommandLineArguments == null)
            {
                return;
            }

            parsedCommandLineArguments.Flags.TryGetValue(AvailableFlags.AlbumId, out var albumId);

            parsedCommandLineArguments.Flags.TryGetValue(AvailableFlags.SearchText, out var searchText);

            if (parsedCommandLineArguments.Resource == AvailableResources.Albums)
            {
                var retrievedAlbums =
                    (await _dataRetrievalService.RetrieveAlbums(albumId as int?, searchText as string)).ToList();

                if (retrievedAlbums.Any())
                {
                    _userInterfaceService.ShowAlbumListing(retrievedAlbums);
                }
                else
                {
                    _userInterfaceService.ShowNoResultsFoundMessage();
                }
            }
            else if (parsedCommandLineArguments.Resource == AvailableResources.Images)
            {
                var retrievedImages =
                    (await _dataRetrievalService.RetrieveImages(albumId as int?, searchText as string))
                    .ToList();

                if (retrievedImages.Any())
                {
                    _userInterfaceService.ShowImageListing(retrievedImages);
                }
                else
                {
                    _userInterfaceService.ShowNoResultsFoundMessage();
                }
            }
        }
        catch (Exception)
        {
            _userInterfaceService.ShowUnhandledExceptionMessage();
        }
    }
}

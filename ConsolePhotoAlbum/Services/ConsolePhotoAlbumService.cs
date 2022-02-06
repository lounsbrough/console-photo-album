namespace ConsolePhotoAlbum.Services;

using Adapters.Interfaces;
using Enums;
using Interfaces;

public class ConsolePhotoAlbumService : IConsolePhotoAlbumService
{
    private readonly IConsoleAdapter _consoleAdapter;
    private readonly IUserInputService _userInputService;
    private readonly IImageRetrievalService _imageRetrievalService;

    public ConsolePhotoAlbumService(
        IConsoleAdapter consoleAdapter,
        IUserInputService userInputService,
        IImageRetrievalService imageRetrievalService)
    {
        _consoleAdapter = consoleAdapter;
        _userInputService = userInputService;
        _imageRetrievalService = imageRetrievalService;
    }

    public async Task RunProgram()
    {
        bool loopAgain;

        do
        {
            loopAgain = await RunMenuLoop();
        }
        while (loopAgain);
    }

    public async Task<bool> RunMenuLoop()
    {
        try
        {
            _userInputService.ShowUserInstructions();

            var parsedUserCommands = _userInputService.GetParsedUserCommands();
            var commandsValid = _userInputService.ValidateUserCommands(parsedUserCommands);

            if (!commandsValid)
            {
                _userInputService.ShowReturnToMenuPrompt();

                return true;
            }

            if (parsedUserCommands.Any(command => command.Command == UserCommands.Exit))
            {
                return false;
            }

            var parsedAlbumCommand = parsedUserCommands.FirstOrDefault(command => command.Command == UserCommands.Album);
            var parsedSearchCommand = parsedUserCommands.FirstOrDefault(command => command.Command == UserCommands.Search);

            int? albumId = null;
            if (int.TryParse(parsedAlbumCommand?.Argument, out var parsedAlbumId))
            {
                albumId = parsedAlbumId;
            }

            var retrievedImages = (await _imageRetrievalService.RetrieveImages(albumId, parsedSearchCommand?.Argument)).ToList();

            if (retrievedImages.Any())
            {
                _userInputService.ShowImageListing(retrievedImages);
            }
            else
            {
                _userInputService.ShowNoImagesFoundMessage();
            }

            _userInputService.ShowReturnToMenuPrompt();
        }
        catch (Exception)
        {
            _consoleAdapter.WriteErrorLine("Unhandled exception occurred.");
            _userInputService.ShowReturnToMenuPrompt();
        }

        return true;
    }
}

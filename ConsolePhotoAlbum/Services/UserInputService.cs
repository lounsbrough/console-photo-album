namespace ConsolePhotoAlbum.Services;

using System;
using ConsolePhotoAlbum.Adapters;

public class UserInputService : IUserInputService
{
    private readonly IConsoleAdapter _consoleAdapter;

    public UserInputService(IConsoleAdapter consoleAdapter)
    {
        _consoleAdapter = consoleAdapter;
    }

    public int GetAlbumId(string[] commandLineArguments)
    {
        string? albumIdInput;

        var userProvidedCommandLineArguments = GetUserProvidedCommandLineArguments(commandLineArguments);

        albumIdInput = userProvidedCommandLineArguments.FirstOrDefault() ?? PromptUserForAlbumId();

        if (int.TryParse(albumIdInput, out int albumId))
        {
            return albumId;
        }

        throw new ArgumentException("Album id must be a number.");
    }

    private static IEnumerable<string> GetUserProvidedCommandLineArguments(string[] commandLineArguments)
    {
        return commandLineArguments.Skip(1);
    }

    private string? PromptUserForAlbumId()
    {
        _consoleAdapter.Write("Please enter album id to retrieve images: ");

        return _consoleAdapter.ReadLine();
    }
}

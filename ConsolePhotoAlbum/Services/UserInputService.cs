using System;
using ConsolePhotoAlbum.Adapters;

namespace ConsolePhotoAlbum.Services;

public class UserInputService : IUserInputService
{
    private readonly IConsoleAdapter _consoleAdapter;

    public UserInputService(IConsoleAdapter consoleAdapter)
    {
        _consoleAdapter = consoleAdapter;
    }

    public string GetUserInput()
    {
        _consoleAdapter.WriteLine("Search options coming soon.");

        return _consoleAdapter.ReadLine();
    }
}

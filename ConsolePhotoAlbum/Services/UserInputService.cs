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
        _consoleAdapter.WriteLine("Please enter album id to retrieve images:");

        return _consoleAdapter.ReadLine()!;
    }
}

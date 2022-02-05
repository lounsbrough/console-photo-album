namespace ConsolePhotoAlbum.Services;

using System;
using ConsolePhotoAlbum.Adapters;
using ConsolePhotoAlbum.DataTransferObjects;
using Newtonsoft.Json;

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
        try
        {
            var albumId = _userInputService.GetAlbumId(Environment.GetCommandLineArgs());

            var albumImages = await _imageRetrievalService.RetrieveImagesInAlbum((int)albumId);
        }
        catch (ArgumentException exception)
        {
            _consoleAdapter.WriteLine(exception.Message);
        }
        catch (Exception)
        {
            _consoleAdapter.WriteLine("Unhandled exception occurred.");
        }
    }
}

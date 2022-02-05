using ConsolePhotoAlbum.Adapters;
using ConsolePhotoAlbum.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConsoleAdapter, ConsoleAdapter>()
    .AddSingleton<IUserInputService, UserInputService>()
    .BuildServiceProvider();

var consolePhotoAlbum = serviceProvider.GetService<IUserInputService>();
consolePhotoAlbum?.GetUserInput();

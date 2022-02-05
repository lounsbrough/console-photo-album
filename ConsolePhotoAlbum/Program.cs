#pragma warning disable SA1200
using ConsolePhotoAlbum.Adapters;
using ConsolePhotoAlbum.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
#pragma warning restore SA1200

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConsolePhotoAlbumService, ConsolePhotoAlbumService>()
    .AddSingleton<IConsoleAdapter, ConsoleAdapter>()
    .AddScoped<IUserInputService, UserInputService>()
    .AddScoped<IImageRetrievalService, ImageRetrievalService>()
    .AddSingleton<HttpClient>()
    .BuildServiceProvider();

var consolePhotoAlbumService = serviceProvider.GetRequiredService<IConsolePhotoAlbumService>();

await consolePhotoAlbumService.RunProgram();

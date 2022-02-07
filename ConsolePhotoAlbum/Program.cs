#pragma warning disable SA1200
using ConsolePhotoAlbum.Adapters;
using ConsolePhotoAlbum.Adapters.Interfaces;
using ConsolePhotoAlbum.Services;
using ConsolePhotoAlbum.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#pragma warning restore SA1200

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConsolePhotoAlbumService, ConsolePhotoAlbumService>()
    .AddSingleton<IConsoleAdapter, ConsoleAdapter>()
    .AddScoped<IUserInputService, UserInputService>()
    .AddScoped<IDataRetrievalService, DataRetrievalService>()
    .AddSingleton<HttpClient>()
    .BuildServiceProvider();

var consolePhotoAlbumService = serviceProvider.GetRequiredService<IConsolePhotoAlbumService>();

await consolePhotoAlbumService.RunProgram();

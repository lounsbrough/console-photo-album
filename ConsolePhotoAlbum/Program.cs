using ConsolePhotoAlbum.Adapters;
using ConsolePhotoAlbum.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConsoleAdapter, ConsoleAdapter>()
    .AddScoped<IUserInputService, UserInputService>()
    .AddScoped<IImageRetrievalService, ImageRetrievalService>()
    .AddSingleton<HttpClient>()
    .BuildServiceProvider();

var userInputService = serviceProvider.GetService<IUserInputService>();
int.TryParse(userInputService?.GetUserInput(), out int albumId);

var imageRetrievalService = serviceProvider.GetService<IImageRetrievalService>();
var albumImages = await imageRetrievalService?.RetrieveImagesInAlbum(albumId);

Console.WriteLine(JsonConvert.SerializeObject(albumImages));

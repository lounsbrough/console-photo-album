namespace ConsolePhotoAlbum.Services.Interfaces
{
    public interface IConsolePhotoAlbumService
    {
        Task RunProgram();

        Task<bool> RunMenuLoop();
    }
}
namespace ConsolePhotoAlbum.Adapters;

public interface IConsoleAdapter
{
    void WriteLine(string output);

    string? ReadLine();
}

namespace ConsolePhotoAlbum.Adapters;

public interface IConsoleAdapter
{
    void Write(string output);

    void WriteLine(string output);

    string? ReadLine();
}

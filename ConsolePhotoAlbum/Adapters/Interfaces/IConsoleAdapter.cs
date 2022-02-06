namespace ConsolePhotoAlbum.Adapters.Interfaces;

public interface IConsoleAdapter
{
    void Clear();

    void Write(string output);

    void WriteLine(string output);

    void WriteWarningLine(string output);

    void WriteInfoLine(string output);

    void WriteErrorLine(string output);

    string? ReadLine();
}

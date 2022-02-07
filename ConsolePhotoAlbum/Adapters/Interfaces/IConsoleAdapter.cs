namespace ConsolePhotoAlbum.Adapters.Interfaces;

public interface IConsoleAdapter
{
    void Write(string output);

    void WriteError(string output);

    void WriteLine(string output);

    void WriteWarningLine(string output);

    void WriteInfoLine(string output);

    void WriteErrorLine(string output);
}

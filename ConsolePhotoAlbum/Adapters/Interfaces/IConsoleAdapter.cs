namespace ConsolePhotoAlbum.Adapters.Interfaces;

public interface IConsoleAdapter
{
    void WriteError(string output);

    void WriteInfoLine(string output);

    void WriteWarningLine(string output);

    void WriteErrorLine(string output);

    void WriteNewLines(int count);
}

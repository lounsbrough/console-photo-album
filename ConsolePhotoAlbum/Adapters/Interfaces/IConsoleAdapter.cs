namespace ConsolePhotoAlbum.Adapters.Interfaces;

public interface IConsoleAdapter
{
    void WriteInfo(string output);

    void WriteError(string output);

    void WriteInfoLine(string output);

    void WriteWarningLine(string output);

    void WriteErrorLine(string output);

    void WriteNewLines(int count);
}

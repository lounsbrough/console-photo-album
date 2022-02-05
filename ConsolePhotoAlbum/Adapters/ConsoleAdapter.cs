using System;

namespace ConsolePhotoAlbum.Adapters;

public class ConsoleAdapter : IConsoleAdapter
{
    public void WriteLine(string output)
    {
        Console.WriteLine(output);
    }

    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}

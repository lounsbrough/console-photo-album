namespace ConsolePhotoAlbum.Adapters;

using System;

public class ConsoleAdapter : IConsoleAdapter
{
    public void Write(string output)
    {
        Console.Write(output);
    }

    public void WriteLine(string output)
    {
        Console.WriteLine(output);
    }

    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}

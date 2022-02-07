namespace ConsolePhotoAlbum.Adapters;

using System;
using Interfaces;

public class ConsoleAdapter : IConsoleAdapter
{
    public void Write(string output)
    {
        Console.Write(output);
    }

    public void WriteError(string output)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(output);
        Console.ResetColor();
    }

    public void WriteLine(string output)
    {
        Console.WriteLine(output);
    }

    public void WriteInfoLine(string output)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(output);
        Console.ResetColor();
    }

    public void WriteWarningLine(string output)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(output);
        Console.ResetColor();
    }

    public void WriteErrorLine(string output)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(output);
        Console.ResetColor();
    }
}

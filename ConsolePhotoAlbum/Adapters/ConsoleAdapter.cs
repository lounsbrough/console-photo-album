namespace ConsolePhotoAlbum.Adapters;

using System;
using Interfaces;

public class ConsoleAdapter : IConsoleAdapter
{
    public void WriteError(string output)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(output);
        Console.ResetColor();
    }

    public void WriteInfoLine(string output)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
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

    public void WriteNewLines(int count)
    {
        var newLines = Enumerable.Repeat(Environment.NewLine, count).ToList();

        Console.Write(string.Join(string.Empty, newLines));
    }
}

namespace ConsolePhotoAlbumTests.Extensions;

using System;
using System.Linq;

public static class TestHelperExtensions
{
    public static string RandomizeCase(this string input)
    {
        var randomizer = new Random();

        return new string(input.Select(x => randomizer.Next() % 2 == 0 ?
            char.IsUpper(x) ? x.ToString().ToLower().First() : x.ToString().ToUpper().First() :
            x).ToArray().ToArray());
    }
}

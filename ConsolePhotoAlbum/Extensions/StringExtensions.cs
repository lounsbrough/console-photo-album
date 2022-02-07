namespace ConsolePhotoAlbum.Extensions;

public static class StringExtensions
{
    public static string RandomizeCase(this string input)
    {
        var randomizer = new Random();

        return new string(input.Select(x =>
        {
            var randomBool = randomizer.Next() % 2 == 0;

            return randomBool ?
                x.ToString().ToLower().First() :
                x.ToString().ToUpper().First();
        }).ToArray());
    }
}

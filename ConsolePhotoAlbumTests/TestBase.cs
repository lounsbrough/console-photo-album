namespace ConsolePhotoAlbumTests;

using AutoFixture;
using ChanceNET;

public class TestBase
{
    protected TestBase()
    {
        Fixture = new Fixture();
        Chance = new Chance();
    }

    protected Fixture Fixture { get; }

    protected Chance Chance { get; }
}

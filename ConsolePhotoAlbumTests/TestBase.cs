namespace ConsolePhotoAlbumTests;

using AutoFixture;
using Xunit;

public class TestBase
{
    public TestBase()
    {
        Fixture = new Fixture();
    }

    public Fixture Fixture { get; }
}

using AutoFixture;
using Xunit;

namespace ConsolePhotoAlbumTests;

public class TestBase
{
    public TestBase()
    {
        Fixture = new Fixture();
    }

    public Fixture Fixture { get; }
}

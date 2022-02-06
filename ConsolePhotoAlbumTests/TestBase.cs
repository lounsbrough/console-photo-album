namespace ConsolePhotoAlbumTests;

using AutoFixture;

public class TestBase
{
    public TestBase()
    {
        Fixture = new Fixture();
    }

    public Fixture Fixture { get; }
}

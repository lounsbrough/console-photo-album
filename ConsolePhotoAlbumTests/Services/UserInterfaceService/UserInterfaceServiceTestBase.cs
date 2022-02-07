namespace ConsolePhotoAlbumTests.Services.UserInterfaceService;

using ConsolePhotoAlbum.Adapters.Interfaces;
using ConsolePhotoAlbum.Services;
using ConsolePhotoAlbumTests;
using NSubstitute;

public class UserInterfaceServiceTestBase : TestBase
{
    protected UserInterfaceServiceTestBase()
    {
        ConsoleAdapterMock = Substitute.For<IConsoleAdapter>();

        SubjectUnderTest = new UserInterfaceService(ConsoleAdapterMock);
    }

    protected IConsoleAdapter ConsoleAdapterMock { get; }

    protected UserInterfaceService SubjectUnderTest { get; }
}

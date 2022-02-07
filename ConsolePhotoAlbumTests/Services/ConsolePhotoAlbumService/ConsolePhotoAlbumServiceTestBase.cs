namespace ConsolePhotoAlbumTests.Services.ConsolePhotoAlbumService;

using ConsolePhotoAlbum.Services;
using ConsolePhotoAlbum.Services.Interfaces;
using NSubstitute;

public class ConsolePhotoAlbumServiceTestBase : TestBase
{
    protected ConsolePhotoAlbumServiceTestBase()
    {
        UserInterfaceServiceMock = Substitute.For<IUserInterfaceService>();
        DataRetrievalServiceMock = Substitute.For<IDataRetrievalService>();

        SubjectUnderTest = new ConsolePhotoAlbumService(
            UserInterfaceServiceMock,
            DataRetrievalServiceMock);
    }

    protected IUserInterfaceService UserInterfaceServiceMock { get; }

    protected IDataRetrievalService DataRetrievalServiceMock { get; }

    protected ConsolePhotoAlbumService SubjectUnderTest { get; }
}

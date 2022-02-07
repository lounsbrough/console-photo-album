namespace ConsolePhotoAlbumTests.Services.UserInterfaceService;

using NSubstitute;
using Xunit;

public class ShowUnhandledExceptionMessage : UserInterfaceServiceTestBase
{
    [Fact]
    public void WhenShowingUnhandledExceptionMessage_ThenShowsCorrectMessage()
    {
        SubjectUnderTest.ShowUnhandledExceptionMessage();

        ConsoleAdapterMock.Received(1).WriteErrorLine("An unexpected error has occured, please try again.");
    }
}

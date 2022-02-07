namespace ConsolePhotoAlbumTests.Services.UserInterfaceService;

using NSubstitute;
using Xunit;

public class ShowNoResultsFoundMessage : UserInterfaceServiceTestBase
{
    [Fact]
    public void WhenShowingNoResultsFoundMessage_ThenShowsCorrectMessage()
    {
        SubjectUnderTest.ShowNoResultsFoundMessage();

        ConsoleAdapterMock.Received(1).WriteWarningLine("No results found.");
    }
}

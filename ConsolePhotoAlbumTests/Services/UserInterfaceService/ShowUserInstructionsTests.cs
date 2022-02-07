namespace ConsolePhotoAlbumTests.Services.UserInterfaceService;

using System.Linq;
using FluentAssertions;
using NSubstitute;
using Xunit;

public class ShowUserInstructions : UserInterfaceServiceTestBase
{
    [Fact]
    public void WhenShowingUserInstructions_ThenShowsExpectedInstructions()
    {
        SubjectUnderTest.ShowUserInstructions();

        var instructions = (string?)ConsoleAdapterMock.ReceivedCalls()
            .First(x => x.GetMethodInfo().Name == nameof(ConsoleAdapterMock.WriteError)).GetArguments()[0];

        instructions.Should().Contain("Please provide one of the following commands to this program:");
        instructions.Should().Contain("get albums [--albumId=123] [--searchText=abc]");
        instructions.Should().Contain("get images [--albumId=123] [--searchText=abc]");
    }
}

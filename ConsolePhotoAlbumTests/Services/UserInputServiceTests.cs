using AutoFixture;
using ConsolePhotoAlbum.Adapters;
using ConsolePhotoAlbum.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ConsolePhotoAlbumTests;

public class UserInputServiceTests : TestBase
{
    private readonly IConsoleAdapter consoleAdapter;

    public UserInputService SubjectUnderTest { get; }

    public UserInputServiceTests()
    {
        consoleAdapter = Substitute.For<IConsoleAdapter>();

        SubjectUnderTest = new UserInputService(consoleAdapter);
    }

    [Fact]
    public void WhenUserEntersInput_ThenReturnsInput()
    {
        var expectedUserInput = Fixture.Create<string>();

        consoleAdapter.ReadLine().Returns(expectedUserInput);

        var actualUserInput = SubjectUnderTest.GetUserInput();

        actualUserInput.Should().Be(expectedUserInput);
    }
}

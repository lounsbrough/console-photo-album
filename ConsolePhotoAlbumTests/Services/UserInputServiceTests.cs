using AutoFixture;
using ConsolePhotoAlbum.Adapters;
using ConsolePhotoAlbum.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ConsolePhotoAlbumTests;

public class UserInputServiceTests : TestBase
{
    private readonly IConsoleAdapter _consoleAdapter;
    private readonly UserInputService _subjectUnderTest;

    public UserInputServiceTests()
    {
        _consoleAdapter = Substitute.For<IConsoleAdapter>();

        _subjectUnderTest = new UserInputService(_consoleAdapter);
    }

    [Fact]
    public void WhenUserEntersInput_ThenReturnsInput()
    {
        var expectedUserInput = Fixture.Create<string>();

        _consoleAdapter.ReadLine().Returns(expectedUserInput);

        var actualUserInput = _subjectUnderTest.GetUserInput();

        actualUserInput.Should().Be(expectedUserInput);
    }
}

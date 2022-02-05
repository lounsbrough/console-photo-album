namespace ConsolePhotoAlbumTests;

using System;
using AutoFixture;
using ConsolePhotoAlbum.Adapters;
using ConsolePhotoAlbum.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

public class UserInputServiceTests : TestBase
{
    private const string NonNumeriExceptionMessage = "Album id must be a number.";
    private readonly IConsoleAdapter _consoleAdapter;
    private readonly UserInputService _subjectUnderTest;
    private int _expectedAlbumId;

    public UserInputServiceTests()
    {
        _consoleAdapter = Substitute.For<IConsoleAdapter>();

        _subjectUnderTest = new UserInputService(_consoleAdapter);
    }

    public class GetAlbumId : UserInputServiceTests
    {
        public class NoUserProvidedCommandLineArguments : GetAlbumId
        {
            private readonly string[] _expectedCommandLineArguments;

            public NoUserProvidedCommandLineArguments()
            {
                _expectedCommandLineArguments = new[] { "program name" };

                _consoleAdapter.ReadLine().Returns(Fixture.Create<int>().ToString());
            }

            [Fact]
            public void WhenGettingAlbumId_PromptsUserForInput_AndReadsUserInput()
            {
                _subjectUnderTest.GetAlbumId(_expectedCommandLineArguments);

                _consoleAdapter.Received(1).Write("Please enter album id to retrieve images: ");
                _consoleAdapter.Received(1).ReadLine();
            }

            [Fact]
            public void GivenUserProvidesNumericInput_WhenGettingAlbumId_ThenSetsUserInputAsAlbumId()
            {
                _expectedAlbumId = Fixture.Create<int>();

                _consoleAdapter.ReadLine().Returns(_expectedAlbumId.ToString());

                var actualAlbumId = _subjectUnderTest.GetAlbumId(_expectedCommandLineArguments);

                actualAlbumId.Should().Be(_expectedAlbumId);
            }

            [Fact]
            public void GivenUserProvidesNonNumericInput_WhenGettingAlbumId_ThenThrowsException()
            {
                _consoleAdapter.ReadLine().Returns("I am not a number");

                Action action = () => _subjectUnderTest.GetAlbumId(_expectedCommandLineArguments);

                action.Should().Throw<ArgumentException>().WithMessage(NonNumeriExceptionMessage);
            }
        }

        public class UserProvidedCommandLineArguments : GetAlbumId
        {
            private readonly string[] _expectedCommandLineArguments;

            public UserProvidedCommandLineArguments()
            {
                _expectedCommandLineArguments = new[]
                {
                    "program name",
                    Fixture.Create<int>().ToString(),
                    "another user value"
                };
            }

            [Fact]
            public void GivenUserProvidesArguments_WhenGettingAlbumId_ThenDoesNotPromptForUserInput()
            {
                _subjectUnderTest.GetAlbumId(_expectedCommandLineArguments);

                _consoleAdapter.DidNotReceive().Write(Arg.Any<string>());
                _consoleAdapter.DidNotReceive().ReadLine();
            }

            [Fact]
            public void GivenUserProvidesNumericFirstArgument_WhenGettingAlbumId_ThenUsesArgumentAsAlbumId()
            {
                _expectedAlbumId = Fixture.Create<int>();

                _expectedCommandLineArguments[1] = _expectedAlbumId.ToString();

                var actualAlbumId = _subjectUnderTest.GetAlbumId(_expectedCommandLineArguments);

                actualAlbumId.Should().Be(_expectedAlbumId);
            }

            [Fact]
            public void GivenUserProvidesNonNumericFirstArgument_WhenGettingAlbumId_ThenThrowsException()
            {
                _expectedCommandLineArguments[1] = "I am not a number";

                Action action = () => _subjectUnderTest.GetAlbumId(_expectedCommandLineArguments);

                action.Should().Throw<ArgumentException>().WithMessage(NonNumeriExceptionMessage);
            }
        }
    }
}

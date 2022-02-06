namespace ConsolePhotoAlbumTests.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ConsolePhotoAlbum.Enums;
using ConsolePhotoAlbum.Services;
using ConsolePhotoAlbumTests;
using FluentAssertions;
using NSubstitute;
using ConsolePhotoAlbum.Adapters.Interfaces;
using ConsolePhotoAlbum.DataTransferObjects;
using Xunit;

public class UserInputServiceTests : TestBase
{
    private readonly IConsoleAdapter _consoleAdapter;
    private readonly UserInputService _subjectUnderTest;

    protected UserInputServiceTests()
    {
        _consoleAdapter = Substitute.For<IConsoleAdapter>();

        _subjectUnderTest = new UserInputService(_consoleAdapter);
    }

    public class ShowUserInstructions : UserInputServiceTests
    {
        [Fact]
        public void WhenShowingUserInstructions_ThenClearsScreenBeforeOutput()
        {
            _subjectUnderTest.ShowUserInstructions();

            _consoleAdapter.Received(1).Clear();
        }

        [Fact]
        public void WhenShowingUserInstructions_ThenShowsExpectedInstructions()
        {
            var albumCommand = AvailableUserCommands.Commands
            .First(command => command.Command == UserCommands.Album);

            var searchCommand = AvailableUserCommands.Commands
                .First(command => command.Command == UserCommands.Search);

            var exitCommand = AvailableUserCommands.Commands
                .First(command => command.Command == UserCommands.Exit);

            _subjectUnderTest.ShowUserInstructions();

            var instructions = (string?)_consoleAdapter.ReceivedCalls()
                .First(x => x.GetMethodInfo().Name == nameof(_consoleAdapter.Write)).GetArguments()[0];

            instructions.Should().Contain("Please choose one or more commands:");
            instructions.Should().Contain($"{albumCommand.Flag} {{id}} - Find images in a given album id.");
            instructions.Should().MatchRegex($@"Example: {albumCommand.Flag} \d*");
            instructions.Should().Contain($"{searchCommand.Flag} {{text}} - Find images with names matching the search text.");
            instructions.Should().MatchRegex($@"Example: {searchCommand.Flag} [a-zA-Z]*");
            instructions.Should().Contain($"{exitCommand.Flag} - Exit this program.");
            instructions.Should().Contain("Enter command:");
        }
    }

    public class ShowReturnToMenuPrompt : UserInputServiceTests
    {
        [Fact]
        public void WhenShowingReturnToMenuPrompt_ThenShowsMessage_AndWaitsForUserInput()
        {
            _subjectUnderTest.ShowReturnToMenuPrompt();

            _consoleAdapter.Received(1).Write($"{Environment.NewLine}{Environment.NewLine}");
            _consoleAdapter.Received(1).WriteLine("Press Enter to return to commands menu...");
            _consoleAdapter.Received(1).ReadLine();
        }
    }

    public class ShowImageListing : UserInputServiceTests
    {
        [Fact]
        public void WhenShowingImageListing_ThenOutputsHeaderAndDetailLines()
        {
            const string headerLine = "Album Id - Id - Title - Image Url";

            var expectedImages = Fixture.CreateMany<Image>(2).ToList();

            _subjectUnderTest.ShowImageListing(expectedImages);

            Received.InOrder(() =>
            {
                _consoleAdapter.WriteLine(headerLine);
                _consoleAdapter.WriteInfoLine($"{expectedImages[0].AlbumId} - {expectedImages[0].Id} - {expectedImages[0].Title} - {expectedImages[0].ImageUrl}");
                _consoleAdapter.WriteInfoLine($"{expectedImages[1].AlbumId} - {expectedImages[1].Id} - {expectedImages[1].Title} - {expectedImages[1].ImageUrl}");
                _consoleAdapter.WriteLine(headerLine);
            });
        }
    }

    public class ShowNoImagesFoundMessage : UserInputServiceTests
    {
        [Fact]
        public void WhenShowingNoImagesFoundMessage_ThenShowsMessage()
        {
            _subjectUnderTest.ShowNoImagesFoundMessage();

            _consoleAdapter.Received(1).Write($"{Environment.NewLine}{Environment.NewLine}");
            _consoleAdapter.Received(1).WriteWarningLine("No images found.");
        }
    }

    public class GetParsedUserCommands : UserInputServiceTests
    {
        [Fact]
        public void GivenUserEntersNothing_WhenGettingParsedUserCommands_ThenReturnsEmptyList()
        {
            _consoleAdapter.ReadLine().Returns(string.Empty);

            var actualParsedUserCommands = _subjectUnderTest.GetParsedUserCommands();

            actualParsedUserCommands.Should().BeEmpty();
        }

        [Fact]
        public void GivenUserEntersNonMatchingString_WhenGettingParsedUserCommands_ThenReturnsEmptyList()
        {
            _consoleAdapter.ReadLine().Returns(Fixture.Create<string>());

            var actualParsedUserCommands = _subjectUnderTest.GetParsedUserCommands();

            actualParsedUserCommands.Should().BeEmpty();
        }

        [Fact]
        public void GivenUserEntersStringMatchingAlbum_WhenGettingParsedUserCommands_ThenReturnsCorrectCommand()
        {
            _consoleAdapter.ReadLine().Returns("abc --album 123 def");

            var expectedParsedUsedCommand = new ParsedUserCommand
            {
                Command = UserCommands.Album,
                HasArgument = true,
                Argument = "123",
                Flag = "--album",
                IsExclusive = false
            };

            var actualParsedUserCommands = _subjectUnderTest.GetParsedUserCommands();

            actualParsedUserCommands.First().Should().BeEquivalentTo(expectedParsedUsedCommand);
        }

        [Fact]
        public void GivenUserEntersStringMatchingSearch_WhenGettingParsedUserCommands_ThenReturnsCorrectCommand()
        {
            _consoleAdapter.ReadLine().Returns("abc --search text def");

            var expectedParsedUsedCommand = new ParsedUserCommand
            {
                Command = UserCommands.Search,
                HasArgument = true,
                Argument = "text",
                Flag = "--search",
                IsExclusive = false
            };

            var actualParsedUserCommands = _subjectUnderTest.GetParsedUserCommands();

            actualParsedUserCommands.First().Should().BeEquivalentTo(expectedParsedUsedCommand);
        }

        [Fact]
        public void GivenUserEntersStringMatchingAll_WhenGettingParsedUserCommands_ThenReturnsCorrectCommand()
        {
            _consoleAdapter.ReadLine().Returns("abc --all def");

            var expectedParsedUsedCommand = new ParsedUserCommand
            {
                Command = UserCommands.All,
                HasArgument = false,
                Argument = string.Empty,
                Flag = "--all",
                IsExclusive = true
            };

            var actualParsedUserCommands = _subjectUnderTest.GetParsedUserCommands();

            actualParsedUserCommands.First().Should().BeEquivalentTo(expectedParsedUsedCommand);
        }

        [Fact]
        public void GivenUserEntersStringMatchingExit_WhenGettingParsedUserCommands_ThenReturnsCorrectCommand()
        {
            _consoleAdapter.ReadLine().Returns("abc --exit def");

            var expectedParsedUsedCommand = new ParsedUserCommand
            {
                Command = UserCommands.Exit,
                HasArgument = false,
                Argument = string.Empty,
                Flag = "--exit",
                IsExclusive = true
            };

            var actualParsedUserCommands = _subjectUnderTest.GetParsedUserCommands();

            actualParsedUserCommands.First().Should().BeEquivalentTo(expectedParsedUsedCommand);
        }
    }

    public class ValidateUserCommands : UserInputServiceTests
    {
        [Fact]
        public void GivenEmptyListOfCommands_WhenValidatingCommands_ThenIsNotValid()
        {
            var expectedUserCommands = new List<ParsedUserCommand>();

            var commandsValid = _subjectUnderTest.ValidateUserCommands(expectedUserCommands);

            commandsValid.Should().BeFalse();
            _consoleAdapter.Received(1).WriteErrorLine("Please choose at least one valid command.");
        }

        [Fact]
        public void GivenAlbumCommand_AndArgument_WhenValidatingCommands_ThenIsValid()
        {
            var albumCommand = AvailableUserCommands.Commands
                .Select(availableCommand => new ParsedUserCommand
                {
                    Command = availableCommand.Command,
                    HasArgument = availableCommand.HasArgument,
                    Argument = Fixture.Create<string>(),
                    Flag = availableCommand.Flag,
                    IsExclusive = availableCommand.IsExclusive
                })
                .First(command => command.Command == UserCommands.Album);

            var expectedUserCommands = new List<ParsedUserCommand> { albumCommand };

            var commandsValid = _subjectUnderTest.ValidateUserCommands(expectedUserCommands);

            commandsValid.Should().BeTrue();
            _consoleAdapter.DidNotReceive().WriteErrorLine(Arg.Any<string>());
        }

        [Fact]
        public void GivenAlbumCommand_AndNoArgument_WhenValidatingCommands_ThenIsNotValid()
        {
            var albumCommand = AvailableUserCommands.Commands
                .Select(availableCommand => new ParsedUserCommand
                {
                    Command = availableCommand.Command,
                    HasArgument = availableCommand.HasArgument,
                    Argument = string.Empty,
                    Flag = availableCommand.Flag,
                    IsExclusive = availableCommand.IsExclusive
                })
                .First(command => command.Command == UserCommands.Album);

            var expectedUserCommands = new List<ParsedUserCommand> { albumCommand };

            var commandsValid = _subjectUnderTest.ValidateUserCommands(expectedUserCommands);

            commandsValid.Should().BeFalse();
            _consoleAdapter.Received(1).WriteErrorLine($"Command {albumCommand.Flag} requires an argument.");
        }

        [Fact]
        public void GivenSearchCommand_AndArgument_WhenValidatingCommands_ThenIsValid()
        {
            var searchCommand = AvailableUserCommands.Commands
                .Select(availableCommand => new ParsedUserCommand
                {
                    Command = availableCommand.Command,
                    HasArgument = availableCommand.HasArgument,
                    Argument = Fixture.Create<string>(),
                    Flag = availableCommand.Flag,
                    IsExclusive = availableCommand.IsExclusive
                })
                .First(command => command.Command == UserCommands.Search);

            var expectedUserCommands = new List<ParsedUserCommand> { searchCommand };

            var commandsValid = _subjectUnderTest.ValidateUserCommands(expectedUserCommands);

            commandsValid.Should().BeTrue();
            _consoleAdapter.DidNotReceive().WriteErrorLine(Arg.Any<string>());
        }

        [Fact]
        public void GivenSearchCommand_AndNoArgument_WhenValidatingCommands_ThenIsNotValid()
        {
            var searchCommand = AvailableUserCommands.Commands
                .Select(availableCommand => new ParsedUserCommand
                {
                    Command = availableCommand.Command,
                    HasArgument = availableCommand.HasArgument,
                    Argument = string.Empty,
                    Flag = availableCommand.Flag,
                    IsExclusive = availableCommand.IsExclusive
                })
                .First(command => command.Command == UserCommands.Search);

            var expectedUserCommands = new List<ParsedUserCommand> { searchCommand };

            var commandsValid = _subjectUnderTest.ValidateUserCommands(expectedUserCommands);

            commandsValid.Should().BeFalse();
            _consoleAdapter.Received(1).WriteErrorLine($"Command {searchCommand.Flag} requires an argument.");
        }

        [Fact]
        public void GivenOnlyAllCommand_WhenValidatingCommands_ThenIsValid()
        {
            var allCommand = AvailableUserCommands.Commands
                .Select(availableCommand => new ParsedUserCommand
                {
                    Command = availableCommand.Command,
                    HasArgument = availableCommand.HasArgument,
                    Argument = null,
                    Flag = availableCommand.Flag,
                    IsExclusive = availableCommand.IsExclusive
                })
                .First(command => command.Command == UserCommands.All);

            var expectedUserCommands = new List<ParsedUserCommand> { allCommand };

            var commandsValid = _subjectUnderTest.ValidateUserCommands(expectedUserCommands);

            commandsValid.Should().BeTrue();
            _consoleAdapter.DidNotReceive().WriteErrorLine(Arg.Any<string>());
        }

        [Fact]
        public void GivenAllCommand_AndAlbumCommand_WhenValidatingCommands_ThenIsNotValid()
        {
            var allCommand = AvailableUserCommands.Commands
                .Select(availableCommand => new ParsedUserCommand
                {
                    Command = availableCommand.Command,
                    HasArgument = availableCommand.HasArgument,
                    Argument = null,
                    Flag = availableCommand.Flag,
                    IsExclusive = availableCommand.IsExclusive
                })
                .First(command => command.Command == UserCommands.All);

            var albumCommand = AvailableUserCommands.Commands
                .Select(availableCommand => new ParsedUserCommand
                {
                    Command = availableCommand.Command,
                    HasArgument = availableCommand.HasArgument,
                    Argument = Fixture.Create<string>(),
                    Flag = availableCommand.Flag,
                    IsExclusive = availableCommand.IsExclusive
                })
                .First(command => command.Command == UserCommands.Album);

            var expectedUserCommands = new List<ParsedUserCommand> { allCommand, albumCommand };

            var commandsValid = _subjectUnderTest.ValidateUserCommands(expectedUserCommands);

            commandsValid.Should().BeFalse();
            _consoleAdapter.Received(1).WriteErrorLine($"Command {allCommand.Flag} may not be combined with other commands.");
        }

        [Fact]
        public void GivenOnlyExitCommand_WhenValidatingCommands_ThenIsValid()
        {
            var exitCommand = AvailableUserCommands.Commands
                .Select(availableCommand => new ParsedUserCommand
                {
                    Command = availableCommand.Command,
                    HasArgument = availableCommand.HasArgument,
                    Argument = null,
                    Flag = availableCommand.Flag,
                    IsExclusive = availableCommand.IsExclusive
                })
                .First(command => command.Command == UserCommands.Exit);

            var expectedUserCommands = new List<ParsedUserCommand> { exitCommand };

            var commandsValid = _subjectUnderTest.ValidateUserCommands(expectedUserCommands);

            commandsValid.Should().BeTrue();
            _consoleAdapter.DidNotReceive().WriteErrorLine(Arg.Any<string>());
        }
    }
}

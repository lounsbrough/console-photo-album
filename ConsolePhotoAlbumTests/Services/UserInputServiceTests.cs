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
using Extensions;
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

    public class ParseCommandLineArguments : UserInputServiceTests
    {
        [Fact]
        public void GivenNoUserArguments_ThenFailsToParse()
        {
            var expectedCommandLineArguments = new[]
            {
                "dll path"
            };

            var parsedCommandLineArguments = _subjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

            _consoleAdapter.Received(1).WriteError(Arg.Is<string>(s =>
                s.Contains("Please provide one of the following commands to this program:")));
            parsedCommandLineArguments.Should().BeNull();
        }

        [Fact]
        public void GivenOneUserArgument_ThenFailsToParse()
        {
            var expectedCommandLineArguments = new[]
            {
                Fixture.Create<string>(),
                Fixture.Create<string>()
            };

            var parsedCommandLineArguments = _subjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

            _consoleAdapter.Received(1).WriteError(Arg.Is<string>(s =>
                s.Contains("Please provide one of the following commands to this program:")));
            parsedCommandLineArguments.Should().BeNull();
        }

        [Fact]
        public void GivenActionInvalid_ThenFailsToParse()
        {
            var actionString = Fixture.Create<string>();

            var expectedCommandLineArguments = new[]
            {
                Fixture.Create<string>(),
                actionString,
                Chance.PickEnum<AvailableResources>().ToString().RandomizeCase()
            };

            var parsedCommandLineArguments = _subjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

            _consoleAdapter.Received(1).WriteErrorLine($"Unknown action \"{actionString}\".");
            parsedCommandLineArguments.Should().BeNull();
        }

        [Fact]
        public void GivenResourceInvalid_ThenFailsToParse()
        {
            var resourceString = Fixture.Create<string>();

            var expectedCommandLineArguments = new[]
            {
                Fixture.Create<string>(),
                Chance.PickEnum<AvailableActions>().ToString().RandomizeCase(),
                resourceString
            };

            var parsedCommandLineArguments = _subjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

            _consoleAdapter.Received(1).WriteErrorLine($"Unknown resource \"{resourceString}\".");
            parsedCommandLineArguments.Should().BeNull();
        }

        [Fact]
        public void GivenValidAction_AndValidResource_ThenParses()
        {
            var expectedAction = Chance.PickEnum<AvailableActions>();
            var expectedResource = Chance.PickEnum<AvailableResources>();

            var expectedParsedArguments = new ParsedCommandLineArguments
            {
                Action = expectedAction,
                Resource = expectedResource
            };

            var expectedCommandLineArguments = new[]
            {
                Fixture.Create<string>(),
                expectedAction.ToString().RandomizeCase(),
                expectedResource.ToString().RandomizeCase()
            };

            var parsedCommandLineArguments = _subjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

            _consoleAdapter.DidNotReceive().WriteErrorLine(Arg.Any<string>());
            parsedCommandLineArguments.Should().BeEquivalentTo(expectedParsedArguments);
        }

        [Fact]
        public void GivenInvalidFlag_ThenFailsToParse()
        {
            var flagString = Fixture.Create<string>();

            var expectedCommandLineArguments = new[]
            {
                Fixture.Create<string>(),
                Chance.PickEnum<AvailableActions>().ToString().RandomizeCase(),
                Chance.PickEnum<AvailableResources>().ToString().RandomizeCase(),
                flagString
            };

            var parsedCommandLineArguments = _subjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

            _consoleAdapter.Received(1).WriteErrorLine($"Unknown flag \"{flagString}\".");
            parsedCommandLineArguments.Should().BeNull();
        }

        [Fact]
        public void GivenValidAction_AndValidResource_AndSearchTextFlag_ThenParses()
        {
            var expectedAction = Chance.PickEnum<AvailableActions>();
            var expectedResource = Chance.PickEnum<AvailableResources>();
            const AvailableFlags expectedFlag = AvailableFlags.SearchText;
            var expectedFlagValue = Fixture.Create<object>();

            var expectedParsedArguments = new ParsedCommandLineArguments
            {
                Action = expectedAction,
                Resource = expectedResource,
                Flags = new Dictionary<AvailableFlags, object>
                {
                    { expectedFlag, expectedFlagValue }
                }
            };

            var expectedCommandLineArguments = new[]
            {
                Fixture.Create<string>(),
                expectedAction.ToString().RandomizeCase(),
                expectedResource.ToString().RandomizeCase(),
                $"--{expectedFlag.ToString().RandomizeCase()}={expectedFlagValue}"
            };

            var parsedCommandLineArguments = _subjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

            _consoleAdapter.DidNotReceive().WriteErrorLine(Arg.Any<string>());
            parsedCommandLineArguments.Should().BeEquivalentTo(expectedParsedArguments);
        }

        [Fact]
        public void GivenValidAction_AndValidResource_AndIntegerAlbumIdFlag_ThenParses()
        {
            var expectedAction = Chance.PickEnum<AvailableActions>();
            var expectedResource = Chance.PickEnum<AvailableResources>();
            const AvailableFlags expectedFlag = AvailableFlags.AlbumId;
            var expectedFlagValue = Fixture.Create<int>();

            var expectedParsedArguments = new ParsedCommandLineArguments
            {
                Action = expectedAction,
                Resource = expectedResource,
                Flags = new Dictionary<AvailableFlags, object>
                {
                    { expectedFlag, expectedFlagValue }
                }
            };

            var expectedCommandLineArguments = new[]
            {
                Fixture.Create<string>(),
                expectedAction.ToString().RandomizeCase(),
                expectedResource.ToString().RandomizeCase(),
                $"--{expectedFlag.ToString().RandomizeCase()}={expectedFlagValue}"
            };

            var parsedCommandLineArguments = _subjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

            _consoleAdapter.DidNotReceive().WriteErrorLine(Arg.Any<string>());
            parsedCommandLineArguments.Should().BeEquivalentTo(expectedParsedArguments);
        }

        [Fact]
        public void GivenValidAction_AndValidResource_AndNonNumericAlbumIdFlag_ThenFailsToParse()
        {
            var expectedAction = Chance.PickEnum<AvailableActions>();
            var expectedResource = Chance.PickEnum<AvailableResources>();
            const AvailableFlags expectedFlag = AvailableFlags.AlbumId;
            var expectedFlagValue = Fixture.Create<string>();

            var expectedCommandLineArguments = new[]
            {
                Fixture.Create<string>(),
                expectedAction.ToString().RandomizeCase(),
                expectedResource.ToString().RandomizeCase(),
                $"--{expectedFlag.ToString().RandomizeCase()}={expectedFlagValue}"
            };

            var parsedCommandLineArguments = _subjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

            _consoleAdapter.Received(1).WriteErrorLine("Flag \"--albumId\" must be a number.");
            parsedCommandLineArguments.Should().BeNull();
        }
    }

    public class ShowUserInstructions : UserInputServiceTests
    {
        [Fact]
        public void WhenShowingUserInstructions_ThenShowsExpectedInstructions()
        {
            _subjectUnderTest.ShowUserInstructions();

            var instructions = (string?)_consoleAdapter.ReceivedCalls()
                .First(x => x.GetMethodInfo().Name == nameof(_consoleAdapter.WriteError)).GetArguments()[0];

            instructions.Should().Contain("Please provide one of the following commands to this program:");
            instructions.Should().Contain("get albums [--albumId=3] [--searchText=abc]");
            instructions.Should().Contain("get images [--albumId=3] [--searchText=abc]");
        }
    }

    public class ShowAlbumListing : UserInputServiceTests
    {
        [Fact]
        public void WhenShowingAlbumListing_ThenOutputsHeaderAndDetailLines()
        {
            const string headerLine = "Id - Title";

            var expectedAlbums = Fixture.CreateMany<Album>(2).ToList();

            _subjectUnderTest.ShowAlbumListing(expectedAlbums);

            Received.InOrder(() =>
            {
                _consoleAdapter.WriteLine(headerLine);
                _consoleAdapter.WriteInfoLine($"{expectedAlbums[0].Id} - {expectedAlbums[0].Title}");
                _consoleAdapter.WriteInfoLine($"{expectedAlbums[1].Id} - {expectedAlbums[1].Title}");
                _consoleAdapter.WriteLine(headerLine);
            });
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
                _consoleAdapter.WriteInfoLine($"{expectedImages[0].AlbumId} - {expectedImages[0].Id} - {expectedImages[0].Title} - {expectedImages[0].Url}");
                _consoleAdapter.WriteInfoLine($"{expectedImages[1].AlbumId} - {expectedImages[1].Id} - {expectedImages[1].Title} - {expectedImages[1].Url}");
                _consoleAdapter.WriteLine(headerLine);
            });
        }
    }

    public class ShowNoResultsFoundMessage : UserInputServiceTests
    {
        [Fact]
        public void WhenShowingNoResultsFoundMessage_ThenShowsMessage()
        {
            _subjectUnderTest.ShowNoResultsFoundMessage();

            _consoleAdapter.Received(1).Write($"{Environment.NewLine}{Environment.NewLine}");
            _consoleAdapter.Received(1).WriteWarningLine("No results found.");
        }
    }
}

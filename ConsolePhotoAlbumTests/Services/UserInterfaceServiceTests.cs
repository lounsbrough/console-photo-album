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

public class UserInterfaceServiceTests : TestBase
{
    private readonly IConsoleAdapter _consoleAdapter;
    private readonly UserInterfaceService _subjectUnderTest;

    protected UserInterfaceServiceTests()
    {
        _consoleAdapter = Substitute.For<IConsoleAdapter>();

        _subjectUnderTest = new UserInterfaceService(_consoleAdapter);
    }

    public class ParseCommandLineArguments : UserInterfaceServiceTests
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

    public class ShowUserInstructions : UserInterfaceServiceTests
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

    public class ShowAlbumListing : UserInterfaceServiceTests
    {
        [Fact]
        public void WhenShowingAlbumListing_ThenOutputsListingTableCorrectly()
        {
            var expectedAlbums = new List<Album>
            {
                new ()
                {
                    Id = 1,
                    Title = "First Album"
                },
                new ()
                {
                    Id = 10,
                    Title = "Another Album"
                }
            };

            _subjectUnderTest.ShowAlbumListing(expectedAlbums);

            Received.InOrder(() =>
            {
                _consoleAdapter.WriteWarningLine("Id | Title        ");
                _consoleAdapter.WriteInfoLine("1  | First Album  ");
                _consoleAdapter.WriteInfoLine("10 | Another Album");
                _consoleAdapter.WriteWarningLine("Id | Title        ");
            });
        }
    }

    public class ShowImageListing : UserInterfaceServiceTests
    {
        [Fact]
        public void WhenShowingImageListing_ThenOutputsListingTableCorrectly()
        {
            var expectedImages = new List<Image>
            {
                new ()
                {
                    AlbumId = 1,
                    Id = 2,
                    Title = "Image in album 1",
                    Url = new Uri("https://awesome.images.com/")
                },
                new ()
                {
                    AlbumId = 10,
                    Id = 20,
                    Title = "Another image in album 10",
                    Url = new Uri("https://cool.images.com/")
                }
            };

            _subjectUnderTest.ShowImageListing(expectedImages);

            Received.InOrder(() =>
            {
                _consoleAdapter.WriteWarningLine("Album Id | Id | Title                     | Image Url                  ");
                _consoleAdapter.WriteInfoLine("1        | 2  | Image in album 1          | https://awesome.images.com/");
                _consoleAdapter.WriteInfoLine("10       | 20 | Another image in album 10 | https://cool.images.com/   ");
                _consoleAdapter.WriteWarningLine("Album Id | Id | Title                     | Image Url                  ");
            });
        }
    }

    public class ShowNoResultsFoundMessage : UserInterfaceServiceTests
    {
        [Fact]
        public void WhenShowingNoResultsFoundMessage_ThenShowsMessage()
        {
            _subjectUnderTest.ShowNoResultsFoundMessage();

            _consoleAdapter.Received(1).WriteWarningLine("No results found.");
        }
    }
}

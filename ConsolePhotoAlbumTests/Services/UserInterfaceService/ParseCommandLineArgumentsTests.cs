namespace ConsolePhotoAlbumTests.Services.UserInterfaceService;

using System.Collections.Generic;
using AutoFixture;
using ConsolePhotoAlbum.DataTransferObjects;
using ConsolePhotoAlbum.Enums;
using ConsolePhotoAlbum.Extensions;
using FluentAssertions;
using NSubstitute;
using Xunit;

public class ParseCommandLineArguments : UserInterfaceServiceTestBase
{
    [Fact]
    public void GivenNoUserArguments_ThenFailsToParse()
    {
        var expectedCommandLineArguments = new[]
        {
            "dll path"
        };

        var parsedCommandLineArguments = SubjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

        ConsoleAdapterMock.Received(1).WriteError(Arg.Is<string>(s =>
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

        var parsedCommandLineArguments = SubjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

        ConsoleAdapterMock.Received(1).WriteError(Arg.Is<string>(s =>
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

        var parsedCommandLineArguments = SubjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

        ConsoleAdapterMock.Received(1).WriteErrorLine($"Unknown action \"{actionString}\".");
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

        var parsedCommandLineArguments = SubjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

        ConsoleAdapterMock.Received(1).WriteErrorLine($"Unknown resource \"{resourceString}\".");
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

        var parsedCommandLineArguments = SubjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

        ConsoleAdapterMock.DidNotReceive().WriteErrorLine(Arg.Any<string>());
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

        var parsedCommandLineArguments = SubjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

        ConsoleAdapterMock.Received(1).WriteErrorLine($"Unknown flag \"{flagString}\".");
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

        var parsedCommandLineArguments = SubjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

        ConsoleAdapterMock.DidNotReceive().WriteErrorLine(Arg.Any<string>());
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

        var parsedCommandLineArguments = SubjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

        ConsoleAdapterMock.DidNotReceive().WriteErrorLine(Arg.Any<string>());
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

        var parsedCommandLineArguments = SubjectUnderTest.ParseCommandLineArguments(expectedCommandLineArguments);

        ConsoleAdapterMock.Received(1).WriteErrorLine("Flag \"--albumId\" must be a number.");
        parsedCommandLineArguments.Should().BeNull();
    }
}

namespace ConsolePhotoAlbumTests.Services.ConsolePhotoAlbumService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using ConsolePhotoAlbum.DataTransferObjects;
using ConsolePhotoAlbum.Enums;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Xunit;

public class RunProgram : ConsolePhotoAlbumServiceTestBase
{
    [Fact]
    public async Task WhenRunningProgram_AndArgumentsNotParsed_ThenAbort()
    {
        UserInterfaceServiceMock.ParseCommandLineArguments(Arg.Any<string[]>()).ReturnsNull();

        await SubjectUnderTest.RunProgram();

        DataRetrievalServiceMock.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task WhenRunningProgram_AndAlbumsRequested_ThenRetrievesAlbums()
    {
        var expectedAlbumId = Fixture.Create<int>();
        var expectedSearchText = Fixture.Create<string>();

        var expectedParsedArguments = new ParsedCommandLineArguments
        {
            Action = AvailableActions.Get,
            Resource = AvailableResources.Albums,
            Flags = new Dictionary<AvailableFlags, object>
            {
                { AvailableFlags.AlbumId, expectedAlbumId },
                { AvailableFlags.SearchText, expectedSearchText }
            }
        };
        UserInterfaceServiceMock.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);

        await SubjectUnderTest.RunProgram();

        await DataRetrievalServiceMock.Received(1).RetrieveAlbums(expectedAlbumId, expectedSearchText);
    }

    [Fact]
    public async Task WhenRunningProgram_AndAlbumsReturned_ThenShowsAlbums()
    {
        var expectedAlbumId = Fixture.Create<int>();
        var expectedSearchText = Fixture.Create<string>();
        var expectedAlbums = Fixture.CreateMany<Album>().ToList();

        var expectedParsedArguments = new ParsedCommandLineArguments
        {
            Action = AvailableActions.Get,
            Resource = AvailableResources.Albums,
            Flags = new Dictionary<AvailableFlags, object>
            {
                { AvailableFlags.AlbumId, expectedAlbumId },
                { AvailableFlags.SearchText, expectedSearchText }
            }
        };
        UserInterfaceServiceMock.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);
        DataRetrievalServiceMock.RetrieveAlbums(expectedAlbumId, expectedSearchText).Returns(expectedAlbums);

        await SubjectUnderTest.RunProgram();

        await DataRetrievalServiceMock.Received(1).RetrieveAlbums(expectedAlbumId, expectedSearchText);
        UserInterfaceServiceMock.Received(1).ShowAlbumListing(Arg.Any<List<Album>>());
        UserInterfaceServiceMock.DidNotReceive().ShowNoResultsFoundMessage();
    }

    [Fact]
    public async Task WhenRunningProgram_AndNoAlbumsReturned_ThenShowsNoResultsFoundMessage()
    {
        var expectedAlbumId = Fixture.Create<int>();
        var expectedSearchText = Fixture.Create<string>();

        var expectedParsedArguments = new ParsedCommandLineArguments
        {
            Action = AvailableActions.Get,
            Resource = AvailableResources.Albums,
            Flags = new Dictionary<AvailableFlags, object>
            {
                { AvailableFlags.AlbumId, expectedAlbumId },
                { AvailableFlags.SearchText, expectedSearchText }
            }
        };
        UserInterfaceServiceMock.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);
        DataRetrievalServiceMock.RetrieveAlbums(expectedAlbumId, expectedSearchText).Returns(new List<Album>());

        await SubjectUnderTest.RunProgram();

        await DataRetrievalServiceMock.Received(1).RetrieveAlbums(expectedAlbumId, expectedSearchText);
        UserInterfaceServiceMock.Received(1).ShowNoResultsFoundMessage();
        UserInterfaceServiceMock.DidNotReceive().ShowAlbumListing(Arg.Any<List<Album>>());
    }

    [Fact]
    public async Task WhenRunningProgram_AndImagesRequested_ThenRetrievesImages()
    {
        var expectedAlbumId = Fixture.Create<int>();
        var expectedSearchText = Fixture.Create<string>();

        var expectedParsedArguments = new ParsedCommandLineArguments
        {
            Action = AvailableActions.Get,
            Resource = AvailableResources.Images,
            Flags = new Dictionary<AvailableFlags, object>
            {
                { AvailableFlags.AlbumId, expectedAlbumId },
                { AvailableFlags.SearchText, expectedSearchText }
            }
        };
        UserInterfaceServiceMock.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);

        await SubjectUnderTest.RunProgram();

        await DataRetrievalServiceMock.Received(1).RetrieveImages(expectedAlbumId, expectedSearchText);
    }

    [Fact]
    public async Task WhenRunningProgram_AndImagesReturned_ThenShowsImages()
    {
        var expectedAlbumId = Fixture.Create<int>();
        var expectedSearchText = Fixture.Create<string>();
        var expectedImages = Fixture.CreateMany<Image>().ToList();

        var expectedParsedArguments = new ParsedCommandLineArguments
        {
            Action = AvailableActions.Get,
            Resource = AvailableResources.Images,
            Flags = new Dictionary<AvailableFlags, object>
            {
                { AvailableFlags.AlbumId, expectedAlbumId },
                { AvailableFlags.SearchText, expectedSearchText }
            }
        };
        UserInterfaceServiceMock.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);
        DataRetrievalServiceMock.RetrieveImages(expectedAlbumId, expectedSearchText).Returns(expectedImages);

        await SubjectUnderTest.RunProgram();

        await DataRetrievalServiceMock.Received(1).RetrieveImages(expectedAlbumId, expectedSearchText);
        UserInterfaceServiceMock.Received(1).ShowImageListing(Arg.Any<List<Image>>());
        UserInterfaceServiceMock.DidNotReceive().ShowNoResultsFoundMessage();
    }

    [Fact]
    public async Task WhenRunningProgram_AndNoImagesReturned_ThenShowsNoResultsFoundMessage()
    {
        var expectedAlbumId = Fixture.Create<int>();
        var expectedSearchText = Fixture.Create<string>();

        var expectedParsedArguments = new ParsedCommandLineArguments
        {
            Action = AvailableActions.Get,
            Resource = AvailableResources.Images,
            Flags = new Dictionary<AvailableFlags, object>
            {
                { AvailableFlags.AlbumId, expectedAlbumId },
                { AvailableFlags.SearchText, expectedSearchText }
            }
        };
        UserInterfaceServiceMock.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);
        DataRetrievalServiceMock.RetrieveImages(expectedAlbumId, expectedSearchText).Returns(new List<Image>());

        await SubjectUnderTest.RunProgram();

        await DataRetrievalServiceMock.Received(1).RetrieveImages(expectedAlbumId, expectedSearchText);
        UserInterfaceServiceMock.Received(1).ShowNoResultsFoundMessage();
        UserInterfaceServiceMock.DidNotReceive().ShowImageListing(Arg.Any<List<Image>>());
    }

    [Fact]
    public async Task WhenRunningProgram_AndAlbumRetrievalThrows_ThenShowsGenericErrorToUser()
    {
        var expectedParsedArguments = new ParsedCommandLineArguments
        {
            Action = AvailableActions.Get,
            Resource = AvailableResources.Albums
        };
        var expectedExceptionMessage = Fixture.Create<string>();

        UserInterfaceServiceMock.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);
        DataRetrievalServiceMock.RetrieveAlbums(null, null).Throws(new Exception(expectedExceptionMessage));

        await SubjectUnderTest.RunProgram();

        UserInterfaceServiceMock.Received(1).ShowUnhandledExceptionMessage();
    }

    [Fact]
    public async Task WhenRunningProgram_AndImageRetrievalThrows_ThenShowsGenericErrorToUser()
    {
        var expectedParsedArguments = new ParsedCommandLineArguments
        {
            Action = AvailableActions.Get,
            Resource = AvailableResources.Images
        };
        var expectedExceptionMessage = Fixture.Create<string>();

        UserInterfaceServiceMock.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);
        DataRetrievalServiceMock.RetrieveImages(null, null).Throws(new Exception(expectedExceptionMessage));

        await SubjectUnderTest.RunProgram();

        UserInterfaceServiceMock.Received(1).ShowUnhandledExceptionMessage();
    }
}

namespace ConsolePhotoAlbumTests.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using ConsolePhotoAlbum.DataTransferObjects;
using ConsolePhotoAlbum.Enums;
using ConsolePhotoAlbum.Services;
using ConsolePhotoAlbum.Services.Interfaces;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

public class ConsolePhotoAlbumServiceTests : TestBase
{
    private readonly IUserInterfaceService _userInterfaceService;
    private readonly IDataRetrievalService _dataRetrievalService;
    private readonly ConsolePhotoAlbumService _subjectUnderTest;

    protected ConsolePhotoAlbumServiceTests()
    {
        _userInterfaceService = Substitute.For<IUserInterfaceService>();
        _dataRetrievalService = Substitute.For<IDataRetrievalService>();

        _subjectUnderTest = new ConsolePhotoAlbumService(
            _userInterfaceService,
            _dataRetrievalService);
    }

    public class RunProgram : ConsolePhotoAlbumServiceTests
    {
        [Fact]
        public async Task WhenRunningProgram_AndArgumentsNotParsed_ThenAbort()
        {
            _userInterfaceService.ParseCommandLineArguments(Arg.Any<string[]>()).ReturnsNull();

            await _subjectUnderTest.RunProgram();

            _dataRetrievalService.ReceivedCalls().Should().BeEmpty();
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
            _userInterfaceService.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);

            await _subjectUnderTest.RunProgram();

            await _dataRetrievalService.Received(1).RetrieveAlbums(expectedAlbumId, expectedSearchText);
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
            _userInterfaceService.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);
            _dataRetrievalService.RetrieveAlbums(expectedAlbumId, expectedSearchText).Returns(expectedAlbums);

            await _subjectUnderTest.RunProgram();

            await _dataRetrievalService.Received(1).RetrieveAlbums(expectedAlbumId, expectedSearchText);
            _userInterfaceService.Received(1).ShowAlbumListing(Arg.Any<List<Album>>());
            _userInterfaceService.DidNotReceive().ShowNoResultsFoundMessage();
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
            _userInterfaceService.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);
            _dataRetrievalService.RetrieveAlbums(expectedAlbumId, expectedSearchText).Returns(new List<Album>());

            await _subjectUnderTest.RunProgram();

            await _dataRetrievalService.Received(1).RetrieveAlbums(expectedAlbumId, expectedSearchText);
            _userInterfaceService.Received(1).ShowNoResultsFoundMessage();
            _userInterfaceService.DidNotReceive().ShowAlbumListing(Arg.Any<List<Album>>());
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
            _userInterfaceService.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);

            await _subjectUnderTest.RunProgram();

            await _dataRetrievalService.Received(1).RetrieveImages(expectedAlbumId, expectedSearchText);
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
            _userInterfaceService.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);
            _dataRetrievalService.RetrieveImages(expectedAlbumId, expectedSearchText).Returns(expectedImages);

            await _subjectUnderTest.RunProgram();

            await _dataRetrievalService.Received(1).RetrieveImages(expectedAlbumId, expectedSearchText);
            _userInterfaceService.Received(1).ShowImageListing(Arg.Any<List<Image>>());
            _userInterfaceService.DidNotReceive().ShowNoResultsFoundMessage();
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
            _userInterfaceService.ParseCommandLineArguments(Arg.Any<string[]>()).Returns(expectedParsedArguments);
            _dataRetrievalService.RetrieveImages(expectedAlbumId, expectedSearchText).Returns(new List<Image>());

            await _subjectUnderTest.RunProgram();

            await _dataRetrievalService.Received(1).RetrieveImages(expectedAlbumId, expectedSearchText);
            _userInterfaceService.Received(1).ShowNoResultsFoundMessage();
            _userInterfaceService.DidNotReceive().ShowImageListing(Arg.Any<List<Image>>());
        }
    }
}

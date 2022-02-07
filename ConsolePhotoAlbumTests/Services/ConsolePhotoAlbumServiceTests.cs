namespace ConsolePhotoAlbumTests.Services;

using System.Collections.Generic;
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
    }
}

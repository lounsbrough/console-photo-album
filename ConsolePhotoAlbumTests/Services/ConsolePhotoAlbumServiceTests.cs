namespace ConsolePhotoAlbumTests.Services;

using System;
using System.Threading.Tasks;
using AutoFixture;
using ConsolePhotoAlbum.Adapters;
using ConsolePhotoAlbum.Services;
using NSubstitute;
using Xunit;

public class ConsolePhotoAlbumServiceTests : TestBase
{
    private readonly IConsoleAdapter _consoleAdapter;
    private readonly IUserInputService _userInputService;
    private readonly IImageRetrievalService _imageRetrievalService;
    private readonly ConsolePhotoAlbumService _subjectUnderTest;

    public ConsolePhotoAlbumServiceTests()
    {
        _consoleAdapter = Substitute.For<IConsoleAdapter>();
        _userInputService = Substitute.For<IUserInputService>();
        _imageRetrievalService = Substitute.For<IImageRetrievalService>();

        _subjectUnderTest = new ConsolePhotoAlbumService(
            _consoleAdapter,
            _userInputService,
            _imageRetrievalService);
    }

    public class RetrieveImagesInAlbum : ConsolePhotoAlbumServiceTests
    {
        private const string GenericExceptionMessage = "Unhandled exception occurred.";
        private int _expectedAlbumId;

        public RetrieveImagesInAlbum()
        {
            _expectedAlbumId = Fixture.Create<int>();
        }

        [Fact]
        public async Task GivenAlbumIdAvailable_WhenRunningProgram_ThenRetrievesImagesFromAlbum()
        {
            _userInputService.GetAlbumId(Arg.Any<string[]>()).Returns(_expectedAlbumId);

            await _subjectUnderTest.RunProgram();

            await _imageRetrievalService.Received(1).RetrieveImagesInAlbum(_expectedAlbumId);
        }

        [Fact]
        public async Task GivenGetAlbumIdThrowsArgumentException_WhenRunningProgram_ThenShowsUserExceptionMessage()
        {
            var expectedExceptionMessage = Fixture.Create<string>();

            _userInputService
                .When(service => service.GetAlbumId(Arg.Any<string[]>()))
                .Do(service =>
                {
                    throw new ArgumentException(expectedExceptionMessage);
                });

            await _subjectUnderTest.RunProgram();

            _consoleAdapter.Received(1).WriteLine(expectedExceptionMessage);
            await _imageRetrievalService.DidNotReceive().RetrieveImagesInAlbum(Arg.Any<int>());
        }

        [Fact]
        public async Task GivenGetAlbumIdThrowsUnexpectedException_WhenRunningProgram_ThenShowsUserGenericMessage()
        {
            _userInputService
                .When(service => service.GetAlbumId(Arg.Any<string[]>()))
                .Do(service =>
                {
                    throw new Exception(Fixture.Create<string>());
                });

            await _subjectUnderTest.RunProgram();

            _consoleAdapter.Received(1).WriteLine(GenericExceptionMessage);
            await _imageRetrievalService.DidNotReceive().RetrieveImagesInAlbum(Arg.Any<int>());
        }

        [Fact]
        public async Task GivenRetrieveImagesInAlbumThrowsUnexpectedException_WhenRunningProgram_ThenShowsUserGenericMessage()
        {
            _userInputService.GetAlbumId(Arg.Any<string[]>()).Returns(_expectedAlbumId);

            _imageRetrievalService
                .When(service => service.RetrieveImagesInAlbum(Arg.Any<int>()))
                .Do(service =>
                {
                    throw new Exception(Fixture.Create<string>());
                });

            await _subjectUnderTest.RunProgram();

            _consoleAdapter.Received(1).WriteLine(GenericExceptionMessage);
            await _imageRetrievalService.Received(1).RetrieveImagesInAlbum(_expectedAlbumId);
        }
    }
}

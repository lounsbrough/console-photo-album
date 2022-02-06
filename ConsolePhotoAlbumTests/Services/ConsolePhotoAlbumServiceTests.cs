namespace ConsolePhotoAlbumTests.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using ConsolePhotoAlbum.Adapters.Interfaces;
using ConsolePhotoAlbum.DataTransferObjects;
using ConsolePhotoAlbum.Enums;
using ConsolePhotoAlbum.Services;
using ConsolePhotoAlbum.Services.Interfaces;
using FluentAssertions;
using NSubstitute;
using Xunit;

public class ConsolePhotoAlbumServiceTests : TestBase
{
    private readonly IConsoleAdapter _consoleAdapter;
    private readonly IUserInputService _userInputService;
    private readonly IImageRetrievalService _imageRetrievalService;
    private readonly ConsolePhotoAlbumService _subjectUnderTest;

    protected ConsolePhotoAlbumServiceTests()
    {
        _consoleAdapter = Substitute.For<IConsoleAdapter>();
        _userInputService = Substitute.For<IUserInputService>();
        _imageRetrievalService = Substitute.For<IImageRetrievalService>();

        _subjectUnderTest = new ConsolePhotoAlbumService(
            _consoleAdapter,
            _userInputService,
            _imageRetrievalService);
    }

    public class RunMenuLoop : ConsolePhotoAlbumServiceTests
    {
        [Fact]
        public async Task WhenRunningMenuLoop_ThenShowsUserInstructions()
        {
            await _subjectUnderTest.RunMenuLoop();

            _userInputService.Received(1).ShowUserInstructions();
        }

        [Fact]
        public async Task WhenRunningMenuLoop_AndCommandsNotValid_ThenShowReturnToMenuPrompt()
        {
            _userInputService.ValidateUserCommands(Arg.Any<List<ParsedUserCommand>>()).Returns(false);

            var continueLoop = await _subjectUnderTest.RunMenuLoop();

            continueLoop.Should().BeTrue();
            await _imageRetrievalService.DidNotReceive().RetrieveImages(Arg.Any<int?>(), Arg.Any<string?>());
            _userInputService.Received(1).ShowReturnToMenuPrompt();
        }

        [Fact]
        public async Task WhenRunningMenuLoop_AndExitCommand_ThenAbortLoop()
        {
            var expectedParsedUserCommands = new List<ParsedUserCommand>
            {
                new () { Command = UserCommands.Exit }
            };

            _userInputService.GetParsedUserCommands().Returns(expectedParsedUserCommands);
            _userInputService.ValidateUserCommands(expectedParsedUserCommands).Returns(true);

            var continueLoop = await _subjectUnderTest.RunMenuLoop();

            continueLoop.Should().BeFalse();
            await _imageRetrievalService.DidNotReceive().RetrieveImages(Arg.Any<int?>(), Arg.Any<string?>());
            _userInputService.DidNotReceive().ShowReturnToMenuPrompt();
        }

        [Fact]
        public async Task WhenRunningMenuLoop_AndAlbumProvided_AndSearchProvided_ThenRetrieveImagesWithParameters()
        {
            var expectedAlbumId = Fixture.Create<int>();
            var expectedSearchText = Fixture.Create<string>();

            var expectedParsedUserCommands = new List<ParsedUserCommand>
            {
                new ()
                {
                    Command = UserCommands.Album,
                    Argument = expectedAlbumId.ToString()
                },
                new ()
                {
                    Command = UserCommands.Search,
                    Argument = expectedSearchText
                }
            };

            _userInputService.GetParsedUserCommands().Returns(expectedParsedUserCommands);
            _userInputService.ValidateUserCommands(expectedParsedUserCommands).Returns(true);

            var continueLoop = await _subjectUnderTest.RunMenuLoop();

            continueLoop.Should().BeTrue();
            await _imageRetrievalService.Received(1).RetrieveImages(expectedAlbumId, expectedSearchText);
            _userInputService.Received(1).ShowReturnToMenuPrompt();
        }

        [Fact]
        public async Task WhenRunningMenuLoop_AndImagesReturned_ThenShowImageListingForUser()
        {
            var expectedImages = Fixture.CreateMany<Image>().ToList();

            _userInputService.GetParsedUserCommands().Returns(new List<ParsedUserCommand>());
            _userInputService.ValidateUserCommands(Arg.Any<List<ParsedUserCommand>>()).Returns(true);
            _imageRetrievalService.RetrieveImages(Arg.Any<int?>(), Arg.Any<string?>())
                .Returns(expectedImages);

            var continueLoop = await _subjectUnderTest.RunMenuLoop();

            continueLoop.Should().BeTrue();
            _userInputService.Received(1).ShowImageListing(Arg.Any<List<Image>>());
            _userInputService.Received(1).ShowReturnToMenuPrompt();
        }

        [Fact]
        public async Task WhenRunningMenuLoop_AndNoImagesReturned_ThenShowNoImagesFoundMessage()
        {
            _userInputService.GetParsedUserCommands().Returns(new List<ParsedUserCommand>());
            _userInputService.ValidateUserCommands(Arg.Any<List<ParsedUserCommand>>()).Returns(true);
            _imageRetrievalService.RetrieveImages(Arg.Any<int?>(), Arg.Any<string?>())
                .Returns(new List<Image>());

            var continueLoop = await _subjectUnderTest.RunMenuLoop();

            continueLoop.Should().BeTrue();
            _userInputService.Received(1).ShowNoImagesFoundMessage();
            _userInputService.Received(1).ShowReturnToMenuPrompt();
        }
    }
}

namespace ConsolePhotoAlbum.Services.Interfaces;

using DataTransferObjects;

public interface IUserInputService
{
    void ShowUserInstructions();

    void ShowReturnToMenuPrompt();

    void ShowImageListing(List<Image> retrievedImages);

    void ShowNoImagesFoundMessage();

    List<ParsedUserCommand> GetParsedUserCommands();

    bool ValidateUserCommands(List<ParsedUserCommand> parsedUserCommands);
}

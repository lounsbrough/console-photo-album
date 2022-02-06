namespace ConsolePhotoAlbum.DataTransferObjects;

using Enums;

public class AvailableUserCommand
{
    public UserCommands Command { get; set; }

    public string? Flag { get; set; }

    public bool HasArgument { get; set; }

    public bool IsExclusive { get; set; }
}

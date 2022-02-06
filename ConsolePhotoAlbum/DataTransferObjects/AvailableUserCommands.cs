namespace ConsolePhotoAlbum.DataTransferObjects;

using Enums;

public static class AvailableUserCommands
{
    public static List<AvailableUserCommand> Commands => new ()
    {
        new AvailableUserCommand
        {
            Command = UserCommands.Exit,
            Flag = "--exit",
            HasArgument = false,
            IsExclusive = true
        },
        new AvailableUserCommand
        {
            Command = UserCommands.Album,
            Flag = "--album",
            HasArgument = true,
            IsExclusive = false
        },
        new AvailableUserCommand
        {
            Command = UserCommands.Search,
            Flag = "--search",
            HasArgument = true,
            IsExclusive = false
        },
        new AvailableUserCommand
        {
            Command = UserCommands.All,
            Flag = "--all",
            HasArgument = false,
            IsExclusive = true
        }
    };
}

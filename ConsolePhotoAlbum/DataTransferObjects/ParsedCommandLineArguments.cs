namespace ConsolePhotoAlbum.DataTransferObjects;

using Enums;

public class ParsedCommandLineArguments
{
    public ParsedCommandLineArguments()
    {
        Flags = new Dictionary<AvailableFlags, object>();
    }

    public AvailableActions Action { get; set; }

    public AvailableResources Resource { get; set; }

    public Dictionary<AvailableFlags, object> Flags { get; set; }
}

namespace ConsolePhotoAlbum.Services;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Adapters.Interfaces;
using Enums;
using DataTransferObjects;
using Interfaces;

public class UserInputService : IUserInputService
{
    private readonly IConsoleAdapter _consoleAdapter;

    public UserInputService(IConsoleAdapter consoleAdapter)
    {
        _consoleAdapter = consoleAdapter;
    }

    public void ShowUserInstructions()
    {
        _consoleAdapter.Clear();

        var newLine = Environment.NewLine;

        var albumCommand = AvailableUserCommands.Commands
            .First(command => command.Command == UserCommands.Album);

        var searchCommand = AvailableUserCommands.Commands
            .First(command => command.Command == UserCommands.Search);

        var allCommand = AvailableUserCommands.Commands
            .First(command => command.Command == UserCommands.All);

        var exitCommand = AvailableUserCommands.Commands
            .First(command => command.Command == UserCommands.Exit);

        var instructions =
            $"Please choose one or more commands:{newLine}{newLine}" +
            $"{albumCommand.Flag} {{id}} - Find images in a given album id.{newLine}" +
            $"  Example: {albumCommand.Flag} 3{newLine}{newLine}" +
            $"{searchCommand.Flag} {{text}} - Find images with names matching the search text.{newLine}" +
            $"  Example: {searchCommand.Flag} velit{newLine}{newLine}" +
            $"{allCommand.Flag} - Find all images.{newLine}{newLine}" +
            $"{exitCommand.Flag} - Exit this program.{newLine}{newLine}" +
            "Enter command: ";

        _consoleAdapter.Write(instructions);
    }

    public void ShowReturnToMenuPrompt()
    {
        WriteNewLines(2);
        _consoleAdapter.WriteLine("Press Enter to return to commands menu...");
        _consoleAdapter.ReadLine();
    }

    public void ShowImageListing(List<Image> retrievedImages)
    {
        const string headerLine = "Album Id - Id - Title - Image Url";

        WriteNewLines(2);
        _consoleAdapter.WriteLine(headerLine);

        WriteNewLines(1);
        foreach (var retrievedImage in retrievedImages)
        {
            _consoleAdapter.WriteInfoLine($"{retrievedImage.AlbumId} - {retrievedImage.Id} - {retrievedImage.Title} - {retrievedImage.ImageUrl}");
        }

        WriteNewLines(1);
        _consoleAdapter.WriteLine(headerLine);
    }

    public void ShowNoImagesFoundMessage()
    {
        WriteNewLines(2);
        _consoleAdapter.WriteWarningLine("No images found.");
    }

    public List<ParsedUserCommand> GetParsedUserCommands()
    {
        var parsedUserCommands = new List<ParsedUserCommand>();

        var userInput = _consoleAdapter.ReadLine() ?? string.Empty;

        foreach (var availableCommand in AvailableUserCommands.Commands)
        {
            var pattern = $"{availableCommand.Flag}{(availableCommand.HasArgument ? @"\s+([^\s]+)" : string.Empty)}";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            var match = regex.Match(userInput);

            if (match.Success)
            {
                parsedUserCommands.Add(new ParsedUserCommand
                {
                    Command = availableCommand.Command,
                    HasArgument = availableCommand.HasArgument,
                    Flag = availableCommand.Flag,
                    IsExclusive = availableCommand.IsExclusive,
                    Argument = match.Groups[1].Value
                });
            }
        }

        return parsedUserCommands;
    }

    public bool ValidateUserCommands(List<ParsedUserCommand> parsedUserCommands)
    {
        if (!parsedUserCommands.Any())
        {
            _consoleAdapter.WriteErrorLine("Please choose at least one valid command.");

            return false;
        }

        var exclusiveCommand = parsedUserCommands.FirstOrDefault(command => command.IsExclusive);

        if (parsedUserCommands.Count > 1 && exclusiveCommand != null)
        {
            _consoleAdapter.WriteErrorLine($"Command {exclusiveCommand.Flag} may not be combined with other commands.");

            return false;
        }

        var commandMissingArgument = parsedUserCommands.FirstOrDefault(command => command.HasArgument && string.IsNullOrWhiteSpace(command.Argument));

        if (commandMissingArgument != null)
        {
            _consoleAdapter.WriteErrorLine($"Command {commandMissingArgument.Flag} requires an argument.");

            return false;
        }

        return true;
    }

    private void WriteNewLines(int count)
    {
        var newLines = Enumerable.Repeat(Environment.NewLine, count).ToList();

        _consoleAdapter.Write(string.Join(string.Empty, newLines));
    }
}

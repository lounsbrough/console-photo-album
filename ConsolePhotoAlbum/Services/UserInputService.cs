namespace ConsolePhotoAlbum.Services;

using System;
using System.Collections.Generic;
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

    public ParsedCommandLineArguments? ParseCommandLineArguments(IEnumerable<string> commandLineArguments)
    {
        var userProvidedArguments = commandLineArguments.Skip(1).ToList();

        if (userProvidedArguments.Count < 2)
        {
            ShowUserInstructions();

            return null;
        }

        var parsedCommandLineArguments = new ParsedCommandLineArguments();

        var actionString = userProvidedArguments[0];
        var resourceString = userProvidedArguments[1];

        if (Enum.TryParse(actionString, true, out AvailableActions action))
        {
            parsedCommandLineArguments.Action = action;
        }
        else
        {
            _consoleAdapter.WriteErrorLine($"Unknown action \"{actionString}\".");

            return null;
        }

        if (Enum.TryParse(resourceString, true, out AvailableResources resource))
        {
            parsedCommandLineArguments.Resource = resource;
        }
        else
        {
            _consoleAdapter.WriteErrorLine($"Unknown resource \"{resourceString}\".");

            return null;
        }

        if (userProvidedArguments.Count <= 2)
        {
            return parsedCommandLineArguments;
        }

        if (!ParseFlagArguments(userProvidedArguments, parsedCommandLineArguments))
        {
            return null;
        }

        return parsedCommandLineArguments;
    }

    public void ShowUserInstructions()
    {
        var newLine = Environment.NewLine;

        var instructions =
            $"Please provide one of the following commands to this program:{newLine}{newLine}" +
            $"get albums [--albumId=3] [--searchText=abc]{newLine}" +
            $"get images [--albumId=3] [--searchText=abc]{newLine}";

        _consoleAdapter.WriteError(instructions);
    }

    public void ShowAlbumListing(List<Album> retrievedAlbums)
    {
        const string headerLine = "Id - Title";

        WriteNewLines(2);
        _consoleAdapter.WriteLine(headerLine);

        WriteNewLines(1);
        foreach (var retrievedAlbum in retrievedAlbums)
        {
            _consoleAdapter.WriteInfoLine($"{retrievedAlbum.Id} - {retrievedAlbum.Title}");
        }

        WriteNewLines(1);
        _consoleAdapter.WriteLine(headerLine);
    }

    public void ShowImageListing(List<Image> retrievedImages)
    {
        const string headerLine = "Album Id - Id - Title - Image Url";

        WriteNewLines(2);
        _consoleAdapter.WriteLine(headerLine);

        WriteNewLines(1);
        foreach (var retrievedImage in retrievedImages)
        {
            _consoleAdapter.WriteInfoLine($"{retrievedImage.AlbumId} - {retrievedImage.Id} - {retrievedImage.Title} - {retrievedImage.Url}");
        }

        WriteNewLines(1);
        _consoleAdapter.WriteLine(headerLine);
    }

    public void ShowNoResultsFoundMessage()
    {
        WriteNewLines(2);
        _consoleAdapter.WriteWarningLine("No results found.");
    }

    private bool ParseFlagArguments(IEnumerable<string> userProvidedArguments, ParsedCommandLineArguments parsedCommandLineArguments)
    {
        foreach (var flagString in userProvidedArguments.Skip(2))
        {
            var flagName = flagString.Replace("--", string.Empty).Split("=").FirstOrDefault();
            var flagValue = string.Join(string.Empty, flagString.Split("=").Skip(1));

            if (Enum.TryParse(flagName, true, out AvailableFlags flag))
            {
                if (flag == AvailableFlags.AlbumId)
                {
                    if (int.TryParse(flagValue, out var parsedAlbumId))
                    {
                        parsedCommandLineArguments.Flags.Add(flag, parsedAlbumId);
                    }
                    else
                    {
                        _consoleAdapter.WriteErrorLine("Flag \"--albumId\" must be a number.");

                        return false;
                    }
                }
                else
                {
                    parsedCommandLineArguments.Flags.Add(flag, flagValue);
                }
            }
            else
            {
                _consoleAdapter.WriteErrorLine($"Unknown flag \"{flagString}\".");

                return false;
            }
        }

        return true;
    }

    private void WriteNewLines(int count)
    {
        var newLines = Enumerable.Repeat(Environment.NewLine, count).ToList();

        _consoleAdapter.Write(string.Join(string.Empty, newLines));
    }
}

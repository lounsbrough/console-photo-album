namespace ConsolePhotoAlbum.Services;

using System;
using System.Collections.Generic;
using Adapters.Interfaces;
using Enums;
using DataTransferObjects;
using Interfaces;

public class UserInterfaceService : IUserInterfaceService
{
    private readonly IConsoleAdapter _consoleAdapter;

    public UserInterfaceService(IConsoleAdapter consoleAdapter)
    {
        _consoleAdapter = consoleAdapter;
    }

    public ParsedCommandLineArguments? ParseCommandLineArguments(IEnumerable<string> commandLineArguments)
    {
        var userProvidedArguments = commandLineArguments.Skip(1).ToList();

        const int minimumUserProvidedArguments = 2;
        if (userProvidedArguments.Count < minimumUserProvidedArguments)
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

        if (userProvidedArguments.Count == minimumUserProvidedArguments)
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
            $"get albums [--albumId=123] [--searchText=abc]{newLine}" +
            $"get images [--albumId=123] [--searchText=abc]{newLine}";

        _consoleAdapter.WriteError(instructions);
    }

    public void ShowAlbumListing(List<Album> retrievedAlbums)
    {
        var headerFields = new[] { "Id", "Title" };

        var detailRecords = retrievedAlbums.Select(album =>
            new[]
            {
                album.Id.ToString(),
                album.Title ?? string.Empty
            }).ToList();

        OutputListingTable(headerFields, detailRecords);
    }

    public void ShowImageListing(List<Image> retrievedImages)
    {
        var headerFields = new[] { "Album Id", "Id", "Title", "Image Url" };

        var detailRecords = retrievedImages.Select(image =>
            new[]
            {
                image.AlbumId.ToString(),
                image.Id.ToString(),
                image.Title ?? string.Empty,
                image.Url?.ToString() ?? string.Empty
            }).ToList();

        OutputListingTable(headerFields, detailRecords);
    }

    public void ShowNoResultsFoundMessage()
    {
        _consoleAdapter.WriteWarningLine("No results found.");
    }

    public void ShowUnhandledExceptionMessage()
    {
        _consoleAdapter.WriteErrorLine("An unexpected error has occured, please try again.");
    }

    private static List<int> GetListingTableColumnWidths(IReadOnlyList<string> headerFields, IReadOnlyList<string[]> detailRecords)
    {
        var maxDataLengths = headerFields.Select((headerField, index) =>
            detailRecords.Select(fields => fields[index].Length).Max());

        return maxDataLengths.Select((dataLength, index) =>
            Math.Max(dataLength, headerFields[index].Length)).ToList();
    }

    private void OutputListingTable(IReadOnlyList<string> headerFields, IReadOnlyList<string[]> detailRecords)
    {
        const string fieldSeparator = " | ";

        var columnWidths = GetListingTableColumnWidths(headerFields, detailRecords);

        var paddedHeaderFields = headerFields.Select((field, index) => field.PadRight(columnWidths[index])).ToList();

        _consoleAdapter.WriteWarningLine(string.Join(fieldSeparator, paddedHeaderFields));

        foreach (var detailRecord in detailRecords)
        {
            var paddedDetailFields = detailRecord.Select((field, index) => field.PadRight(columnWidths[index])).ToList();
            _consoleAdapter.WriteInfoLine(string.Join(fieldSeparator, paddedDetailFields));
        }

        _consoleAdapter.WriteWarningLine(string.Join(fieldSeparator, paddedHeaderFields));
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
}

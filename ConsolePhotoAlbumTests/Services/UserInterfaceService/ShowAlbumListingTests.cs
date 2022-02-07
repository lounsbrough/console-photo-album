namespace ConsolePhotoAlbumTests.Services.UserInterfaceService;

using System.Collections.Generic;
using ConsolePhotoAlbum.DataTransferObjects;
using NSubstitute;
using Xunit;

public class ShowAlbumListing : UserInterfaceServiceTestBase
{
    [Fact]
    public void WhenShowingAlbumListing_ThenOutputsListingTableCorrectly()
    {
        var expectedAlbums = new List<Album>
        {
            new ()
            {
                Id = 1,
                Title = "First Album"
            },
            new ()
            {
                Id = 10,
                Title = "Another Album"
            }
        };

        SubjectUnderTest.ShowAlbumListing(expectedAlbums);

        Received.InOrder(() =>
        {
            ConsoleAdapterMock.WriteWarningLine("Id | Title        ");
            ConsoleAdapterMock.WriteInfoLine("1  | First Album  ");
            ConsoleAdapterMock.WriteInfoLine("10 | Another Album");
            ConsoleAdapterMock.WriteWarningLine("Id | Title        ");
        });
    }
}

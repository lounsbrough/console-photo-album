namespace ConsolePhotoAlbumTests.Services.UserInterfaceService;

using System;
using System.Collections.Generic;
using ConsolePhotoAlbum.DataTransferObjects;
using NSubstitute;
using Xunit;

public class ShowImageListing : UserInterfaceServiceTestBase
{
    [Fact]
    public void WhenShowingImageListing_ThenOutputsListingTableCorrectly()
    {
        var expectedImages = new List<Image>
        {
            new ()
            {
                AlbumId = 1,
                Id = 2,
                Title = "Image in album 1",
                Url = new Uri("https://awesome.images.com/")
            },
            new ()
            {
                AlbumId = 10,
                Id = 20,
                Title = "Another image in album 10",
                Url = new Uri("https://cool.images.com/")
            }
        };

        SubjectUnderTest.ShowImageListing(expectedImages);

        Received.InOrder(() =>
        {
            ConsoleAdapterMock.WriteWarningLine("Album Id | Id | Title                     | Image Url                  ");
            ConsoleAdapterMock.WriteInfoLine("1        | 2  | Image in album 1          | https://awesome.images.com/");
            ConsoleAdapterMock.WriteInfoLine("10       | 20 | Another image in album 10 | https://cool.images.com/   ");
            ConsoleAdapterMock.WriteWarningLine("Album Id | Id | Title                     | Image Url                  ");
        });
    }
}

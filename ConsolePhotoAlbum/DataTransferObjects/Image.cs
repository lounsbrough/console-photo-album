namespace ConsolePhotoAlbum.DataTransferObjects;

using System;

public class Image
{
    public int AlbumId { get; set; }

    public int Id { get; set; }

    public string? Title { get; set; }

    public Uri? ImageUrl { get; set; }
}

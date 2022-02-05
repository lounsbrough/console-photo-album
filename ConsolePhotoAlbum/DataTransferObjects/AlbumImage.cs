namespace ConsolePhotoAlbum.DataTransferObjects;

using System;

public class AlbumImage
{
    public int AlbumId { get; set; }

    public int Id { get; set; }

    public string? Title { get; set; }

    public Uri? ImageUrl { get; set; }

    public Uri? ThumbnailUrl { get; set; }
}

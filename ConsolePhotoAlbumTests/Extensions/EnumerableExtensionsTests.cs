namespace ConsolePhotoAlbumTests.Extensions;

using ConsolePhotoAlbum.Extensions;
using FluentAssertions;
using Xunit;

public class EnumerableExtensionsTests : TestBase
{
    [Fact]
    public void Given()
    {
        var expectedEnumerable = new[]
        {
            "first",
            "second"
        };

        foreach (var (item, index) in expectedEnumerable.WithIndex())
        {
            expectedEnumerable[index].Should().Be(item);
        }
    }
}

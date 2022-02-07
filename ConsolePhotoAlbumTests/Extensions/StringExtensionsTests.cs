namespace ConsolePhotoAlbumTests.Extensions;

using System;
using ConsolePhotoAlbum.Extensions;
using FluentAssertions;
using Xunit;

public class StringExtensionsTests : TestBase
{
    [Fact]
    public void GivenString_WhenCallingRandomizeCase_ThenCaseIsNotAllLowerOrUpper()
    {
        var originalString = Chance.String(10);

        originalString.RandomizeCase().Should().NotBeLowerCased();
        originalString.RandomizeCase().Should().NotBeUpperCased();
    }

    [Fact]
    public void GivenString_WhenCallingRandomizeCase_ThenNewStringIsChangedButOnlyDiffersInCase()
    {
        var originalString = Chance.String(10);

        originalString.RandomizeCase().Should().NotBe(originalString);
        originalString.RandomizeCase().Equals(originalString, StringComparison.OrdinalIgnoreCase).Should().BeTrue();
    }
}

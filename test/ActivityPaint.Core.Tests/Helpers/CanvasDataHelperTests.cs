using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Helpers;

namespace ActivityPaint.Core.Tests.Helpers;

public class CanvasDataHelperTests
{
    [Fact]
    public void ConvertToString_WhenNullArray_ShouldThrowArgumentNullException()
    {
        // Arrage
        var action = () => CanvasDataHelper.ConvertToString(null!);

        // Act & Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ConvertToString_WhenEmptyArray_ShouldBeEmptyString()
    {
        // Arrange
        var input = Enumerable.Empty<IntensityEnum>();

        // Act
        var result = CanvasDataHelper.ConvertToString(input);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ConvertToString_WhenValidArray_ShouldBeValidString()
    {
        // Arrange
        var input = new IntensityEnum[]
        {
            IntensityEnum.Level0,
            IntensityEnum.Level1,
            IntensityEnum.Level2,
            IntensityEnum.Level3,
            IntensityEnum.Level4,
            IntensityEnum.Level0,
            IntensityEnum.Level0
        };

        // Act
        var result = CanvasDataHelper.ConvertToString(input);

        // Assert
        result.Should().Be("0123400");
    }

    [Fact]
    public void ConvertToList_WhenNullString_ShouldThrowArgumentNullException()
    {
        // Arrage
        var action = () => CanvasDataHelper.ConvertToList(null!);

        // Act & Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ConvertToList_WhenEmptyString_ShouldBeEmptyArray()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = CanvasDataHelper.ConvertToList(input);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ConvertToList_WhenValidString_ShouldBeValidArray()
    {
        // Arrange
        var input = "01234";
        var expected = new IntensityEnum[]
        {
            IntensityEnum.Level0,
            IntensityEnum.Level1,
            IntensityEnum.Level2,
            IntensityEnum.Level3,
            IntensityEnum.Level4,
        };

        // Act
        var result = CanvasDataHelper.ConvertToList(input);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
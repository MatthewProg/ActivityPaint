using ActivityPaint.Core.Entities;
using ActivityPaint.Core.Enums;

namespace ActivityPaint.Core.Tests.Entities;

public class PresetTests
{
    [Fact]
    public void CanvasDataString_WhenEmptyArray_ShouldBeEmptyString()
    {
        // Arrange & Act
        var model = new Preset
        {
            CanvasData = [],
        };

        // Assert
        model.CanvasDataString.Should().BeEmpty();
    }

    [Fact]
    public void CanvasDataString_WhenValidArray_ShouldBeValidString()
    {
        // Arrange
        var array = new IntensityEnum[]
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
        var model = new Preset
        {
            CanvasData = array,
        };

        // Assert
        model.CanvasDataString.Should().Be("0123400");
    }

    [Fact]
    public void SetCanvasDataFromString_WhenNullString_ShouldThrowArgumentException()
    {
        // Arrage
        var model = new Preset();
        var action = () => model.SetCanvasDataFromString(null!);

        // Act & Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void SetCanvasDataFromString_WhenEmptyString_ShouldBeEmptyArray()
    {
        // Arrange
        var input = string.Empty;
        var model = new Preset
        {
            CanvasData = [ IntensityEnum.Level0 ],
        };

        // Act
        model.SetCanvasDataFromString(input);

        // Assert
        model.CanvasData.Should().BeEmpty();
    }

    [Fact]
    public void SetCanvasDataFromString_WhenValidString_ShouldBeValidArray()
    {
        // Arrange
        var input = "01234";
        var model = new Preset();
        var expected = new IntensityEnum[]
        {
            IntensityEnum.Level0,
            IntensityEnum.Level1,
            IntensityEnum.Level2,
            IntensityEnum.Level3,
            IntensityEnum.Level4,
        };

        // Act
        model.SetCanvasDataFromString(input);

        // Assert
        model.CanvasData.Should().BeEquivalentTo(expected);
    }
}
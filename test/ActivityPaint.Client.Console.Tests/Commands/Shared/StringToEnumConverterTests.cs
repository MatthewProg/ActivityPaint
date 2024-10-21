using ActivityPaint.Application.BusinessLogic.Image.Models;
using ActivityPaint.Client.Console.Commands.Shared;

namespace ActivityPaint.Client.Console.Tests.Commands.Shared;

public class StringToEnumConverterTests
{
    [Fact]
    public void CanConvertFrom_WhenCorrectTypes_ShouldBeTrue()
    {
        // Arrange
        var inputType = typeof(string);
        var converter = new StringToEnumConverter<ModeEnum>();

        // Act
        var result = converter.CanConvertFrom(inputType);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanConvertTo_WhenCorrectTypes_ShouldBeTrue()
    {
        // Arrange
        var destinationType = typeof(ModeEnum);
        var converter = new StringToEnumConverter<ModeEnum>();

        // Act
        var result = converter.CanConvertTo(destinationType);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("DARK", ModeEnum.Dark)]
    [InlineData("light", ModeEnum.Light)]
    public void ConvertFrom_WhenCorrectValue_ShouldConvert(string input, ModeEnum expected)
    {
        // Arrange
        var converter = new StringToEnumConverter<ModeEnum>();

        // Act
        var result = converter.ConvertFrom(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(ModeEnum.Dark, "Dark")]
    [InlineData(ModeEnum.Light, "Light")]
    public void ConvertTo_WhenCorrectValue_ShouldConvert(ModeEnum input, string expected)
    {
        // Arrange
        var destinationType = typeof(ModeEnum);
        var converter = new StringToEnumConverter<ModeEnum>();

        // Act
        var result = converter.ConvertTo(input, destinationType);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(99)]
    [InlineData(null)]
    [InlineData("incorrect")]
    public void ConvertFrom_WhenIncorrectValue_ShouldThrow(object? input)
    {
        // Arrange
        var converter = new StringToEnumConverter<ModeEnum>();
        var action = () => converter.ConvertFrom(input!);

        // Act & Assert
        action.Should().Throw<NotSupportedException>();
    }

    [Theory]
    [InlineData("no")]
    [InlineData(null)]
    [InlineData((ModeEnum)99)]
    public void ConvertTo_WhenIncorrectValue_ShouldThrow(object? input)
    {
        // Arrange
        var converter = new StringToEnumConverter<ModeEnum>();
        var action = () => converter.ConvertTo(input!, typeof(ModeEnum));

        // Act & Assert
        action.Should().Throw<NotSupportedException>();
    }
}

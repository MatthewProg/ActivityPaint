using ActivityPaint.Application.BusinessLogic.Image.Models;
using ActivityPaint.Client.Console.Commands.Preview;

namespace ActivityPaint.Client.Console.Tests.Commands.Preview;

public class PreviewSaveCommandSettingsTests
{
    [Fact]
    public void Validate_WhenAllPropsAreCorrect_ShouldValidateSuccessfully()
    {
        // Arrange
        var model = GetValidModel();

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenModeOverwriteIsNull_ShouldValidateSuccessfully()
    {
        // Arrange
        var model = GetValidModel();
        model.ModeOverwrite = null;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("https://wrong.com/test.json")]
    public void Validate_WhenIncorrectInputPath_ShouldFailValidation(string? path)
    {
        // Arrange
        var model = GetValidModel();
        model.InputPath = path!;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("https://wrong.com/test.json")]
    public void Validate_WhenIncorrectOutputPath_ShouldFailValidation(string? path)
    {
        // Arrange
        var model = GetValidModel();
        model.OutputPath = path!;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    private static PreviewSaveCommandSettings GetValidModel() => new()
    {
        InputPath = "C:/tmp/test.json",
        OutputPath = "C:/tmp/output.png",
        ModeOverwrite = ModeEnum.Dark,
        Overwrite = true
    };
}

using ActivityPaint.Client.Console.Commands.Save;

namespace ActivityPaint.Client.Console.Tests.Commands.Save;

public class GenerateNewCommandSettingsTests
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
    public void Validate_WhenIncorrectCanvasDataString_ShouldFailValidation()
    {
        // Arrange
        var model = GetValidModel();
        model.CanvasDataString = null!;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("2020")]
    public void Validate_WhenIncorrectStartDateString_ShouldFailValidation(string? startDate)
    {
        // Arrange
        var model = GetValidModel();
        model.StartDateString = startDate!;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void Validate_WhenIncorrectName_ShouldFailValidation()
    {
        // Arrange
        var model = GetValidModel();
        model.Name = null!;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("https://wrong.com/test.json")]
    public void Validate_WhenIncorrectPath_ShouldFailValidation(string? path)
    {
        // Arrange
        var model = GetValidModel();
        model.Path = path!;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    private static SaveCommandSettings GetValidModel() => new()
    {
        CanvasDataString = "01234",
        StartDateString = "2024-01-01",
        IsDarkModeDefault = true,
        Name = "Test",
        Path = "C:/tmp/test.json"
    };
}

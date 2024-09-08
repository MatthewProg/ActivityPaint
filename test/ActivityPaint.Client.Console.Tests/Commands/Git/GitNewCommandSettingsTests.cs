using ActivityPaint.Client.Console.Commands.Git;

namespace ActivityPaint.Client.Console.Tests.Commands.Git;

public class GitNewCommandSettingsTests
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
    [InlineData("")]
    [InlineData(null)]
    public void Validate_WhenIncorrectMessageFormat_ShouldFailValidation(string? message)
    {
        // Arrange
        var model = GetValidModel();
        model.MessageFormat = message!;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    private static GitNewCommandSettings GetValidModel() => new()
    {
        CanvasDataString = "01234",
        StartDateString = "2024-01-01",
        Name = "Test",
        MessageFormat = "Abcd",
        OutputPath = "C:/tmp/output",
        Overwrite = false,
    };
}

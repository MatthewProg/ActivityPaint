using ActivityPaint.Client.Console.Commands.Git;

namespace ActivityPaint.Client.Console.Tests.Commands.Git;

public class GitLoadCommandSettingsTests
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

    private static GitLoadCommandSettings GetValidModel() => new()
    {
        InputPath = "C:/tmp/test.json",
        MessageFormat = "Abcd",
        OutputPath = "C:/tmp/output",
        Overwrite = false,
    };
}

using ActivityPaint.Client.Console.Commands.Generate;

namespace ActivityPaint.Client.Console.Tests.Commands.Generate;

public class GenerateLoadCommandSettingsTests
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
    public void Validate_WhenIncorrectAuthorEmail_ShouldFailValidation(string? email)
    {
        // Arrange
        var model = GetValidModel();
        model.AuthorEmail = email!;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_WhenIncorrectAuthorFullName_ShouldFailValidation(string? name)
    {
        // Arrange
        var model = GetValidModel();
        model.AuthorFullName = name!;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
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

    private static GenerateLoadCommandSettings GetValidModel() => new()
    {
        AuthorEmail = "test@example.com",
        AuthorFullName = "John Doe",
        InputPath = "C:/tmp/test.json",
        MessageFormat = "Abcd",
        OutputPath = "C:/tmp/output",
        Overwrite = false,
        ZipMode = false,
    };
}

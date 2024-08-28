using ActivityPaint.Client.Console.Commands.Generate;

namespace ActivityPaint.Client.Console.Tests.Commands.Generate;

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

    private static GenerateNewCommandSettings GetValidModel() => new()
    {
        CanvasDataString = "01234",
        StartDateString = "2024-01-01",
        Name = "Test",
        AuthorEmail = "test@example.com",
        AuthorFullName = "John Doe",
        MessageFormat = "Abcd",
        OutputPath = "C:/tmp/output",
        Overwrite = false,
        ZipMode = false,
    };
}

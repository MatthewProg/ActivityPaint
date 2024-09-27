using ActivityPaint.Application.BusinessLogic.Files;

namespace ActivityPaint.Application.BusinessLogic.Tests.Files;

public class SaveTextToFileCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var validator = new SaveTextToFileCommandValidator();
        var model = GetValidModel();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenTextIsNull_ShouldBeInvalid()
    {
        // Arrange
        var validator = new SaveTextToFileCommandValidator();
        var model = GetValidModel() with { Text = null! };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Text);
    }

    [Fact]
    public void WhenPathIsNotCorrect_ShouldBeInvalid()
    {
        // Arrange
        var validator = new SaveTextToFileCommandValidator();
        var model = GetValidModel() with { Path = @"C:\sd*?s.txt" };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Path);
    }

    private static SaveTextToFileCommand GetValidModel() => new(
        Text: "Sample\nMultiline\nContent",
        Path: @"C:\Temp\file.txt",
        Overwrite: true
    );
}

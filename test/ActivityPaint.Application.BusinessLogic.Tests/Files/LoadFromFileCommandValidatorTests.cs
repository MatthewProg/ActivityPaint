using ActivityPaint.Application.BusinessLogic.Files;

namespace ActivityPaint.Application.BusinessLogic.Tests.Files;

public class LoadFromFileCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var model = new LoadFromFileCommand(@"C:\test\file.json");
        var validator = new LoadFromFileCommandValidator();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenPathIsNull_ShouldBeValid()
    {
        // Arrange
        var model = new LoadFromFileCommand(null);
        var validator = new LoadFromFileCommandValidator();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenPathIsNotCorrect_ShouldBeInvalid()
    {
        // Arrange
        var model = new LoadFromFileCommand(@"C:\sd*?s.txt");
        var validator = new LoadFromFileCommandValidator();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Path);
    }
}

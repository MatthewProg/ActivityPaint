using ActivityPaint.Application.BusinessLogic.Preset;

namespace ActivityPaint.Application.BusinessLogic.Tests.Preset;

public class LoadPresetCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var validator = new LoadPresetCommandValidator();
        var model = GetValidModel();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenPathIsNull_ShouldBeValid()
    {
        // Arrange
        var validator = new LoadPresetCommandValidator();
        var model = GetValidModel() with { Path = null };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenPathIsNotCorrect_ShouldBeInvalid()
    {
        // Arrange
        var validator = new LoadPresetCommandValidator();
        var model = GetValidModel() with { Path = @"C:\sd*?s.txt" };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Path);
    }

    private static LoadPresetCommand GetValidModel() => new(
        Path: @"C:\tmp\preset.json"
    );
}

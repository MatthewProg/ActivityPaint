using ActivityPaint.Application.BusinessLogic.Preset;

namespace ActivityPaint.Application.BusinessLogic.Tests.Preset;

public class ParsePresetCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        using var stream = new MemoryStream();
        var model = new ParsePresetCommand(stream);
        var validator = new ParsePresetCommandValidator();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenStreamIsNull_ShouldBeInvalid()
    {
        // Arrange
        var validator = new ParsePresetCommandValidator();
        var model = new ParsePresetCommand(null!);

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PresetStream);
    }

    [Fact]
    public void WhenCannotReadStream_ShouldBeInvalid()
    {
        // Arrange
        var validator = new ParsePresetCommandValidator();
        var stream = new MemoryStream();
        stream.Dispose();
        var model = new ParsePresetCommand(stream);

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PresetStream);
    }
}

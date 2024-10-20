using ActivityPaint.Application.BusinessLogic.Image;
using ActivityPaint.Application.BusinessLogic.Image.Models;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Application.DTOs.Preset;

namespace ActivityPaint.Application.BusinessLogic.Tests.Image;

public class GeneratePreviewImageCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new GeneratePreviewImageCommandValidator([presetMock.Object]);
        var model = GetValidModel();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenPresetIsNull_ShouldBeInvalid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.Never());
        var validator = new GeneratePreviewImageCommandValidator([presetMock.Object]);
        var model = GetValidModel() with { Preset = null! };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldHaveValidationErrorFor(x => x.Preset);
    }

    [Fact]
    public void WhenModeOverwriteIsNull_ShouldBeValid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new GeneratePreviewImageCommandValidator([presetMock.Object]);
        var model = GetValidModel() with { ModeOverwrite = null };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenModeOverwriteIsInvalidEnum_ShouldBeInvalid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new GeneratePreviewImageCommandValidator([presetMock.Object]);
        var model = GetValidModel() with { ModeOverwrite = (ModeEnum)99 };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldHaveValidationErrorFor(x => x.ModeOverwrite);
    }

    private static GeneratePreviewImageCommand GetValidModel() => new(
        Preset: new("Test", DateTime.Now, true, []),
        ModeOverwrite: ModeEnum.Light
    );
}

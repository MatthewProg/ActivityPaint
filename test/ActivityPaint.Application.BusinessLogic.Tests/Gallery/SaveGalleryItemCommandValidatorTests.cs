using ActivityPaint.Application.BusinessLogic.Gallery;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Application.DTOs.Preset;

namespace ActivityPaint.Application.BusinessLogic.Tests.Gallery;

public class SaveGalleryItemCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new SaveGalleryItemCommandValidator([presetMock.Object]);
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
        var validator = new SaveGalleryItemCommandValidator([presetMock.Object]);
        var model = GetValidModel() with { Preset = null! };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldHaveValidationErrorFor(x => x.Preset);
    }

    private static SaveGalleryItemCommand GetValidModel() => new(
        Preset: new("Test", DateTime.Now, true, [])
    );
}

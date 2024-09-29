using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Application.DTOs.Preset;

namespace ActivityPaint.Application.BusinessLogic.Tests.Preset;

public class SavePresetCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new SavePresetCommandValidator([presetMock.Object]);
        var model = GetValidModel();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenPathIsNull_ShouldBeValid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new SavePresetCommandValidator([presetMock.Object]);
        var model = GetValidModel() with { Path = null };

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
        var validator = new SavePresetCommandValidator([presetMock.Object]);
        var model = GetValidModel() with { Preset = null! };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldHaveValidationErrorFor(x => x.Preset);
    }

    [Fact]
    public void WhenPathIsNotCorrect_ShouldBeInvalid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new SavePresetCommandValidator([presetMock.Object]);
        var model = GetValidModel() with { Path = @"C:\sd*?s.txt" };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldHaveValidationErrorFor(x => x.Path);
    }

    private static SavePresetCommand GetValidModel() => new(
        Preset: new("Test", DateTime.Now, true, []),
        Path: @"C:\Temp\file.txt",
        Overwrite: true
    );
}

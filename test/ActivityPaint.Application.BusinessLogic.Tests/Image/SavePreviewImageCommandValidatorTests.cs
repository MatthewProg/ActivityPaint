﻿using ActivityPaint.Application.BusinessLogic.Image;
using ActivityPaint.Application.BusinessLogic.Image.Models;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Application.DTOs.Preset;

namespace ActivityPaint.Application.BusinessLogic.Tests.Image;

public class SavePreviewImageCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new SavePreviewImageCommandValidator([presetMock.Object]);
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
        var validator = new SavePreviewImageCommandValidator([presetMock.Object]);
        var model = GetValidModel() with { Preset = null! };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldHaveValidationErrorFor(x => x.Preset);
    }

    [Fact]
    public void WhenDarkModeOverwriteIsNull_ShouldBeValid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new SavePreviewImageCommandValidator([presetMock.Object]);
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
        var validator = new SavePreviewImageCommandValidator([presetMock.Object]);
        var model = GetValidModel() with { ModeOverwrite = (ModeEnum)99 };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldHaveValidationErrorFor(x => x.ModeOverwrite);
    }

    [Fact]
    public void WhenPathIsNull_ShouldBeValid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new SavePreviewImageCommandValidator([presetMock.Object]);
        var model = GetValidModel() with { Path = null };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenPathIsNotCorrect_ShouldBeInvalid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new SavePreviewImageCommandValidator([presetMock.Object]);
        var model = GetValidModel() with { Path = @"C:\sd*?s.txt" };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        presetMock.VerifyAll();
        result.ShouldHaveValidationErrorFor(x => x.Path);
    }

    private static SavePreviewImageCommand GetValidModel() => new(
        Preset: new("Test", DateTime.Now, true, []),
        ModeOverwrite: ModeEnum.Light,
        Path: @"C:\Temp\file.txt",
        Overwrite: true
    );
}

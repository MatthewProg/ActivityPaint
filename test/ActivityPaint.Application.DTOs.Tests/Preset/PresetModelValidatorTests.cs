using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;

namespace ActivityPaint.Application.DTOs.Tests.Preset;

public class PresetModelValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var validator = new PresetModelValidator();
        var model = GetValidModel();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Name_WhenNullOrEmpty_ShouldBeInvalid(string? name)
    {
        // Arrange
        var validator = new PresetModelValidator();
        var model = GetValidModel() with
        {
            Name = name!,
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CanvasData_WhenNull_ShouldBeInvalid()
    {
        // Arrange
        var validator = new PresetModelValidator();
        var model = GetValidModel() with
        {
            CanvasData = null!,
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CanvasData);
    }

    [Fact]
    public void CanvasData_WhenInvalidEnumValues_ShouldBeInvalid()
    {
        // Arrange
        var validator = new PresetModelValidator();
        var model = GetValidModel() with
        {
            CanvasData = [(IntensityEnum)9],
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CanvasData);
    }

    private static PresetModel GetValidModel() => new(
        Name: "abc",
        StartDate: DateTime.Today,
        IsDarkModeDefault: true,
        CanvasData: [IntensityEnum.Level4]
    );
}

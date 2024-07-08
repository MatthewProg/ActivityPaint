using ActivityPaint.Core.Entities;
using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Validators;

namespace ActivityPaint.Core.Tests.Validators;

public class PresetValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Name_WhenNullOrEmpty_ShouldBeInvalid(string name)
    {
        // Arrange
        var validator = new PresetValidator();
        var model = new Preset()
        {
            Name = name,
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
        var validator = new PresetValidator();
        var model = new Preset()
        {
            Name = "abc",
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
        var validator = new PresetValidator();
        var model = new Preset()
        {
            Name = "abc",
            CanvasData = [ (IntensityEnum)9 ],
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CanvasData);
    }

    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var validator = new PresetValidator();
        var model = new Preset()
        {
            Name = "abc",
            Id = new Guid(),
            StartDate = DateTime.Today,
            IsDarkModeDefault = true,
            IsDeleted = false,
            CanvasData = [ IntensityEnum.Level4 ],
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

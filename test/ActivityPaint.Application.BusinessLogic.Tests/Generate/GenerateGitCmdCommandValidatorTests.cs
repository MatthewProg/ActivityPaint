using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Application.DTOs.Preset;

namespace ActivityPaint.Application.BusinessLogic.Tests.Generate;

public class GenerateGitCmdCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var mock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new GenerateGitCmdCommandValidator([mock.Object]);
        var model = GetValidModel();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
        mock.VerifyAll();
    }

    [Fact]
    public void WhenMessageFormatNull_ShouldBeValid()
    {
        // Arrange
        var mock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var validator = new GenerateGitCmdCommandValidator([mock.Object]);
        var model = GetValidModel() with { MessageFormat = null };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
        mock.VerifyAll();
    }

    [Fact]
    public void WhenPresetIsNull_ShouldBeInvalid()
    {
        // Arrange
        var mock = ValidatorMockFactory.CreateValid<PresetModel>(Times.Never());
        var validator = new GenerateGitCmdCommandValidator([mock.Object]);
        var model = GetValidModel() with { Preset = null! };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Preset);
        mock.VerifyAll();
    }

    private static GenerateGitCmdCommand GetValidModel() => new(
        Preset: new("Name", DateTime.Now, true, []),
        MessageFormat: "abc"
    );
}

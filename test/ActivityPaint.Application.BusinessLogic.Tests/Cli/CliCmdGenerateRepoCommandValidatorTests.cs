using ActivityPaint.Application.BusinessLogic.Cli;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;

namespace ActivityPaint.Application.BusinessLogic.Tests.Cli;

public class CliCmdGenerateRepoCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var authorMock = ValidatorMockFactory.CreateValid<AuthorModel>(Times.AtLeastOnce());
        var validator = new CliCmdGenerateRepoCommandValidator([presetMock.Object], [authorMock.Object]);
        var model = GetValidModel();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
        presetMock.VerifyAll();
        authorMock.VerifyAll();
    }

    [Fact]
    public void WhenMessageFormatNull_ShouldBeValid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var authorMock = ValidatorMockFactory.CreateValid<AuthorModel>(Times.AtLeastOnce());
        var validator = new CliCmdGenerateRepoCommandValidator([presetMock.Object], [authorMock.Object]);
        var model = GetValidModel() with { MessageFormat = null };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
        presetMock.VerifyAll();
        authorMock.VerifyAll();
    }

    [Fact]
    public void WhenPresetIsNull_ShouldBeInvalid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.Never());
        var authorMock = ValidatorMockFactory.CreateValid<AuthorModel>(Times.AtLeastOnce());
        var validator = new CliCmdGenerateRepoCommandValidator([presetMock.Object], [authorMock.Object]);
        var model = GetValidModel() with { Preset = null! };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Preset);
        presetMock.VerifyAll();
        authorMock.VerifyAll();
    }

    [Fact]
    public void WhenAuthorIsNull_ShouldBeInvalid()
    {
        // Arrange
        var presetMock = ValidatorMockFactory.CreateValid<PresetModel>(Times.AtLeastOnce());
        var authorMock = ValidatorMockFactory.CreateValid<AuthorModel>(Times.Never());
        var validator = new CliCmdGenerateRepoCommandValidator([presetMock.Object], [authorMock.Object]);
        var model = GetValidModel() with { Author = null! };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Author);
        presetMock.VerifyAll();
        authorMock.VerifyAll();
    }

    private static CliCmdGenerateRepoCommand GetValidModel() => new(
        Preset: new("Name", DateTime.Now, true, []),
        Author: new("email@example.com", "John Doe"),
        MessageFormat: "abc"
    );
}

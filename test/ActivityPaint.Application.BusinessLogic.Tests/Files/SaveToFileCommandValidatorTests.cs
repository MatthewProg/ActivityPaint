using ActivityPaint.Application.BusinessLogic.Files;

namespace ActivityPaint.Application.BusinessLogic.Tests.Files;

public class SaveToFileCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        using var stream = new MemoryStream();
        var model = GetValidModel(stream);
        var validator = new SaveToFileCommandValidator();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void WhenSuggestedFileNameIsInvalid_ShouldBeInvalid(string? suggestedFileName)
    {
        // Arrange
        using var stream = new MemoryStream();
        var model = GetValidModel(stream) with { SuggestedFileName = suggestedFileName! };
        var validator = new SaveToFileCommandValidator();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SuggestedFileName);
    }

    [Fact]
    public void WhenStreamIsNull_ShouldBeInvalid()
    {
        // Arrange
        var model = GetValidModel(null!);
        var validator = new SaveToFileCommandValidator();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DataStream);
    }

    [Fact]
    public void WhenCannotReadStream_ShouldBeInvalid()
    {
        // Arrange
        var stream = new MemoryStream();
        stream.Dispose();
        var model = GetValidModel(stream);
        var validator = new SaveToFileCommandValidator();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DataStream);
    }

    [Fact]
    public void WhenPathIsNull_ShouldBeValid()
    {
        // Arrange
        using var stream = new MemoryStream();
        var model = GetValidModel(stream) with { Path = null };
        var validator = new SaveToFileCommandValidator();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenPathIsNotCorrect_ShouldBeInvalid()
    {
        // Arrange
        using var stream = new MemoryStream();
        var model = GetValidModel(stream) with { Path = @"C:\sd*?s.txt" };
        var validator = new SaveToFileCommandValidator();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Path);
    }

    private static SaveToFileCommand GetValidModel(Stream data) => new(
        DataStream: data,
        SuggestedFileName: "file.txt",
        Path: @"C:\Temp\file.txt",
        Overwrite: true
    );
}

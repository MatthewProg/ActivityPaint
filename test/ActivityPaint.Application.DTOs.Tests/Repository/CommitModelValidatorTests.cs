using ActivityPaint.Application.DTOs.Repository;

namespace ActivityPaint.Application.DTOs.Tests.Repository;

public class CommitModelValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var validator = new CommitModelValidator();
        var model = GetValidModel();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Message_WhenNullOrEmpty_ShouldBeInvalid(string? message)
    {
        // Arrange
        var validator = new CommitModelValidator();
        var model = GetValidModel() with
        {
            Message = message!,
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Message);
    }

    private static CommitModel GetValidModel() => new(
        Message: "Example message",
        DateTime: DateTimeOffset.Now
    );
}

using ActivityPaint.Application.BusinessLogic.Repository;

namespace ActivityPaint.Application.BusinessLogic.Tests.Repository;

public class UpdateRepositoryConfigCommandValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var validator = new UpdateRepositoryConfigCommandValidator();
        var model = GetValidModel();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenModelIsNull_ShouldBeInvalid()
    {
        // Arrange
        var validator = new UpdateRepositoryConfigCommandValidator();
        var model = GetValidModel() with { Model = null! };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Model);
    }

    private static UpdateRepositoryConfigCommand GetValidModel() => new(
        Model: new("{name} commit", "test@example.com", "Unit Test")
    );
}

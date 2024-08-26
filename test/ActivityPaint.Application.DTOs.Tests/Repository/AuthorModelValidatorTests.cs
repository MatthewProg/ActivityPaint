using ActivityPaint.Application.DTOs.Repository;

namespace ActivityPaint.Application.DTOs.Tests.Repository;

public class AuthorModelValidatorTests
{
    [Fact]
    public void WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var validator = new AuthorModelValidator();
        var model = GetValidModel();

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void FullName_WhenNullOrEmpty_ShouldBeInvalid(string? name)
    {
        // Arrange
        var validator = new AuthorModelValidator();
        var model = GetValidModel() with
        {
            FullName = name!,
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Email_WhenNullOrEmpty_ShouldBeInvalid(string? email)
    {
        // Arrange
        var validator = new AuthorModelValidator();
        var model = GetValidModel() with
        {
            Email = email!,
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("abc.com")]
    [InlineData("@example.com")]
    public void Email_WhenIncorrect_ShouldBeInvalid(string email)
    {
        // Arrange
        var validator = new AuthorModelValidator();
        var model = GetValidModel() with
        {
            Email = email,
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    private static AuthorModel GetValidModel() => new(
        Email: "test@example.com",
        FullName: "John Doe"
    );
}

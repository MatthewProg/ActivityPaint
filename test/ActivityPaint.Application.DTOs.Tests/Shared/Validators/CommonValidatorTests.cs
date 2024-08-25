using ActivityPaint.Application.DTOs.Shared.Validators;

namespace ActivityPaint.Application.DTOs.Tests.Shared.Validators;

public class CommonValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void ValidateRequired_WhenNullOrWhiteSpaceString_ShouldFail(string? value)
    {
        // Act
        var isValid = CommonValidator.ValidateRequired(value, out var result);

        // Assert
        isValid.Should().BeFalse();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ValidateRequired_WhenValidString_ShouldSucceed()
    {
        // Arrange
        var value = "abc";

        // Act
        var isValid = CommonValidator.ValidateRequired(value, out var result);

        // Assert
        isValid.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ValidateRequired_WhenNullObject_ShouldFail()
    {
        // Arrange
        List<string>? value = null;

        // Act
        var isValid = CommonValidator.ValidateRequired(value, out var result);

        // Assert
        isValid.Should().BeFalse();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ValidateRequired_WhenNotNullObject_ShouldSucceed()
    {
        // Arrange
        List<string>? value = [];

        // Act
        var isValid = CommonValidator.ValidateRequired(value, out var result);

        // Assert
        isValid.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("2024")]
    [InlineData("2024-1-01")]
    [InlineData("9999999-12-01")]
    [InlineData("2024-01-01T00:00:00.000Z")]
    public void ValidateDateString_WhenIncorrectString_ShouldFail(string? value)
    {
        // Arrange
        var format = "yyyy-MM-dd";

        // Act
        var isValid = CommonValidator.ValidateDateString(value, format, out var result);

        // Assert
        isValid.Should().BeFalse();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ValidateDateString_WhenValidString_ShouldSucceed()
    {
        // Arrange
        var value = "2024-06-03";
        var format = "yyyy-MM-dd";

        // Act
        var isValid = CommonValidator.ValidateDateString(value, format, out var result);

        // Assert
        isValid.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(@"C:/test?.json")]
    [InlineData(@"C:/>dir/test.json")]
    [InlineData("https://www.google.com/test.json")]
    public void ValidatePath_WhenIncorrectString_ShouldFail(string? value)
    {
        // Act
        var isValid = CommonValidator.ValidatePath(value, out var result);

        // Assert
        isValid.Should().BeFalse();
        result.IsSuccess.Should().BeFalse();
    }

    [Theory]
    [InlineData("C:/dir")]
    [InlineData("test.json")]
    [InlineData("directory")]
    [InlineData(@".\test.json")]
    [InlineData("C:/test.json")]
    [InlineData(@"C:\test.json")]
    [InlineData(@"\\TEST\dir\test.json")]
    public void ValidatePath_WhenCorrectString_ShouldSucceed(string? value)
    {
        // Act
        var isValid = CommonValidator.ValidatePath(value, out var result);

        // Assert
        isValid.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
    }
}

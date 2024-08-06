using ActivityPaint.Client.Console.Validators;

namespace ActivityPaint.Client.Console.Tests.Validators;

public class OptionsValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void ValidateRequired_WhenNullOrWhiteSpaceString_ShouldFail(string? value)
    {
        // Act
        var isValid = OptionsValidator.ValidateRequired(value, string.Empty, out var result);

        // Assert
        isValid.Should().BeFalse();
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ValidateRequired_WhenValidString_ShouldSucceed()
    {
        // Arrange
        var value = "abc";

        // Act
        var isValid = OptionsValidator.ValidateRequired(value, string.Empty, out var result);

        // Assert
        isValid.Should().BeTrue();
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void ValidateRequired_WhenNullObject_ShouldFail()
    {
        // Arrange
        List<string>? value = null;

        // Act
        var isValid = OptionsValidator.ValidateRequired(value, string.Empty, out var result);

        // Assert
        isValid.Should().BeFalse();
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ValidateRequired_WhenNotNullObject_ShouldSucceed()
    {
        // Arrange
        List<string>? value = [];

        // Act
        var isValid = OptionsValidator.ValidateRequired(value, string.Empty, out var result);

        // Assert
        isValid.Should().BeTrue();
        result.Successful.Should().BeTrue();
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
        var isValid = OptionsValidator.ValidateDateString(value, format, string.Empty, out var result);

        // Assert
        isValid.Should().BeFalse();
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ValidateDateString_WhenValidString_ShouldSucceed()
    {
        // Arrange
        var value = "2024-06-03";
        var format = "yyyy-MM-dd";

        // Act
        var isValid = OptionsValidator.ValidateDateString(value, format, string.Empty, out var result);

        // Assert
        isValid.Should().BeTrue();
        result.Successful.Should().BeTrue();
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
        var isValid = OptionsValidator.ValidatePath(value, string.Empty, out var result);

        // Assert
        isValid.Should().BeFalse();
        result.Successful.Should().BeFalse();
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
        var isValid = OptionsValidator.ValidatePath(value, string.Empty, out var result);

        // Assert
        isValid.Should().BeTrue();
        result.Successful.Should().BeTrue();
    }
}

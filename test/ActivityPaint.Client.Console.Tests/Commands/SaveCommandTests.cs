using ActivityPaint.Client.Console.Commands.Save;

namespace ActivityPaint.Client.Console.Tests.Commands;

public class SaveCommandTests
{
    [Fact]
    public void SaveCommandSettings_WhenAllPropsAreCorrect_ShouldValidateSuccessfully()
    {
        // Arrange
        var model = GetValidModel();

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void SaveCommandSettings_WhenIncorrectCanvasData_ShouldValidateSuccessfully()
    {
        // Arrange
        var model = GetValidModel();
        model.CanvasData = null;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("2020")]
    public void SaveCommandSettings_WhenIncorrectStartDate_ShouldValidateSuccessfully(string? startDate)
    {
        // Arrange
        var model = GetValidModel();
        model.StartDateString = startDate;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void SaveCommandSettings_WhenIncorrectName_ShouldValidateSuccessfully()
    {
        // Arrange
        var model = GetValidModel();
        model.Name = null;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("https://wrong.com/test.json")]
    public void SaveCommandSettings_WhenIncorrectPath_ShouldValidateSuccessfully(string? path)
    {
        // Arrange
        var model = GetValidModel();
        model.Path = path;

        // Act
        var result = model.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    private static SaveCommandSettings GetValidModel() => new()
    {
        CanvasData = "01234",
        StartDateString = "2024-01-01",
        IsDarkModeDefault = true,
        Name = "Test",
        Path = "C:/tmp/test.json"
    };
}

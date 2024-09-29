using ActivityPaint.Application.BusinessLogic.Cli;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;

namespace ActivityPaint.Application.BusinessLogic.Tests.Cli;

public class CliCmdSavePresetCommandTests
{
    [Fact]
    public async Task Handle_WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var preset = GetValidPreset();
        var command = new CliCmdSavePresetCommand(preset);
        var expected = "ap-cli.exe save --name \"Example name\" --start-date 2023-01-01 --data eAFiZGBiYGYBAAAA//8= --dark-mode --output \"Example name.json\"";
        var service = new CliCmdSavePresetCommandHandler();

        // Act
        var result = await service.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    [Fact]
    public async Task Handle_WhenNotDarkMode_ShouldNotContainFlag()
    {
        // Arrange
        var preset = GetValidPreset() with { IsDarkModeDefault = false };
        var command = new CliCmdSavePresetCommand(preset);
        var expected = "ap-cli.exe save --name \"Example name\" --start-date 2023-01-01 --data eAFiZGBiYGYBAAAA//8= --output \"Example name.json\"";
        var service = new CliCmdSavePresetCommandHandler();

        // Act
        var result = await service.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    private static PresetModel GetValidPreset() => new(
        Name: "Example name",
        StartDate: new DateTime(2023, 1, 1),
        IsDarkModeDefault: true,
        CanvasData: [
            IntensityEnum.Level1,
            IntensityEnum.Level0,
            IntensityEnum.Level2,
            IntensityEnum.Level0,
            IntensityEnum.Level3,
            IntensityEnum.Level4
        ]
    );
}

using ActivityPaint.Application.BusinessLogic.Cli;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;

namespace ActivityPaint.Application.BusinessLogic.Tests.Cli;

public class CliCmdGenerateGitCommandTests
{
    [Fact]
    public async Task Handle_WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var preset = GetValidPreset();
        var command = new CliCmdGenerateGitCommand(preset, "{name} commit");
        var expected = """ap-cli.exe git --message "{name} commit" --output "Example name.txt" new --name "Example name" --start-date 2023-01-01 --data eAFiZGBiYGYBAAAA//8=""";
        var service = new CliCmdGenerateGitCommandHandler();

        // Act
        var result = await service.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    [Fact]
    public async Task Handle_WhenMessageIsNull_ShouldNotContainParameter()
    {
        // Arrange
        var preset = GetValidPreset();
        var command = new CliCmdGenerateGitCommand(preset, null);
        var expected = """ap-cli.exe git --output "Example name.txt" new --name "Example name" --start-date 2023-01-01 --data eAFiZGBiYGYBAAAA//8=""";
        var service = new CliCmdGenerateGitCommandHandler();

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

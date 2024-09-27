using ActivityPaint.Application.BusinessLogic.Cli;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Enums;

namespace ActivityPaint.Application.BusinessLogic.Tests.Cli;

public class CliCmdGenerateRepoCommandTests
{
    [Fact]
    public async Task Handle_WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var preset = GetValidPreset();
        var author = GetValidAuthor();
        var command = new CliCmdGenerateRepoCommand(preset, author, "{name} commit");
        var expected = """ap-cli.exe generate --message "{name} commit" --author-name "Unit Test" --author-email "test@example.com" --zip --output "Example name.zip" new --name "Example name" --start-date 2023-01-01 --data eAFiZGBiYGYBAAAA//8=""";
        var service = new CliCmdGenerateRepoCommandHandler();

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
        var author = GetValidAuthor();
        var command = new CliCmdGenerateRepoCommand(preset, author, null);
        var expected = """ap-cli.exe generate --author-name "Unit Test" --author-email "test@example.com" --zip --output "Example name.zip" new --name "Example name" --start-date 2023-01-01 --data eAFiZGBiYGYBAAAA//8=""";
        var service = new CliCmdGenerateRepoCommandHandler();

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

    private static AuthorModel GetValidAuthor() => new(
        Email: "test@example.com",
        FullName: "Unit Test"
    );
}

using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.BusinessLogic.Generate.Services;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;

namespace ActivityPaint.Application.BusinessLogic.Tests.Generate;

public class GenerateGitCmdCommandTests
{
    private readonly Mock<ICommitsService> _commitsServiceMock = new();
    [Fact]
    public async Task Handle_WhenAllCorrect_ShouldBeValid()
    {
        // Arrange
        var preset = GetValidPreset();
        var messageFormat = "{name} commit";

        _commitsServiceMock.Setup(x => x.GenerateCommits(It.Is<PresetModel>(x => x.Equals(preset)),
                                                         It.Is<string?>(x => x == messageFormat)))
                           .Returns([
                               new("Name commit #1", new DateTimeOffset(2020, 1, 1, 12, 0, 0, TimeSpan.FromHours(1))),
                               new("Name commit #2", new DateTimeOffset(2020, 1, 2, 12, 0, 0, TimeSpan.FromHours(1))),
                               new("Name commit #3", new DateTimeOffset(2020, 1, 2, 12, 0, 0, TimeSpan.FromHours(1)))
                           ])
                           .Verifiable(Times.Once);

        var expected =
            "git commit --allow-empty --no-verify --date=2020-01-01T12:00:00.0000000+01:00 -m \"Name commit #1\";\n" +
            "git commit --allow-empty --no-verify --date=2020-01-02T12:00:00.0000000+01:00 -m \"Name commit #2\";\n" +
            "git commit --allow-empty --no-verify --date=2020-01-02T12:00:00.0000000+01:00 -m \"Name commit #3\";";

        var command = new GenerateGitCmdCommand(preset, messageFormat);
        var service = new GenerateGitCmdCommandHandler(_commitsServiceMock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        _commitsServiceMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    private static PresetModel GetValidPreset() => new(
        Name: "Name",
        StartDate: new DateTime(2020, 1, 1),
        IsDarkModeDefault: true,
        CanvasData: [IntensityEnum.Level1, IntensityEnum.Level2]
    );
}

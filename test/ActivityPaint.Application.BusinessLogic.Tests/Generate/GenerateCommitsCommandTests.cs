using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.BusinessLogic.Generate.Services;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Enums;

namespace ActivityPaint.Application.BusinessLogic.Tests.Generate;

public class GenerateCommitsCommandTests
{
    private readonly Mock<ICommitsService> _commitsServiceMock = new();

    [Theory]
    [InlineData(null)]
    [InlineData("{name} commit")]
    public async Task Handle_WhenAllCorrect_ShouldBeValid(string? messageFormat)
    {
        // Arrange
        var preset = GetValidPreset();
        var expected = new List<CommitModel> { new("Sample", DateTimeOffset.Now) };

        _commitsServiceMock.Setup(x => x.GenerateCommits(It.Is<PresetModel>(x => x.Equals(preset)),
                                                         It.Is<string?>(x => x == messageFormat)))
                           .Returns(expected)
                           .Verifiable(Times.Once);

        var command = new GenerateCommitsCommand(preset, messageFormat);
        var service = new GenerateCommitsCommandHandler(_commitsServiceMock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        _commitsServiceMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(expected);
    }

    private static PresetModel GetValidPreset() => new(
        Name: "Name",
        StartDate: DateTime.Now,
        IsDarkModeDefault: true,
        CanvasData: [IntensityEnum.Level1]
    );
}

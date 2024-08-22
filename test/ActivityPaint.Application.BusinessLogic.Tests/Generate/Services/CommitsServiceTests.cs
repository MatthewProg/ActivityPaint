using ActivityPaint.Application.Abstractions.Repository.Models;
using ActivityPaint.Application.BusinessLogic.Generate.Services;
using ActivityPaint.Application.DTOs.Models;
using ActivityPaint.Core.Enums;

namespace ActivityPaint.Application.BusinessLogic.Tests.Generate.Services;

public class CommitsServiceTests
{
    [Fact]
    public void GenerateCommits_WhenMultipleTokensUsed_ShouldHaveValidMessages()
    {
        // Arrange
        var service = new CommitsService();
        var model = GetSampleValidPreset();
        var format = "Test - {name} ({current_total}/{total_count})";
        List<CommitModel> expected = [
            new ("Test - Example name (1/10)", new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test - Example name (2/10)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test - Example name (3/10)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test - Example name (4/10)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test - Example name (5/10)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test - Example name (6/10)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test - Example name (7/10)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test - Example name (8/10)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test - Example name (9/10)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test - Example name (10/10)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
        ];

        // Act
        var result = service.GenerateCommits(model, format);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    public static PresetModel GetSampleValidPreset() => new(
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

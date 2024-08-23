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

    [Fact]
    public void GenerateCommits_NameToken_ShouldHaveValidMessages()
    {
        // Arrange
        var service = new CommitsService();
        var model = GetSampleValidPreset();
        var format = "Test ({name})";
        List<CommitModel> expected = [
            new ("Test (Example name)", new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (Example name)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (Example name)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (Example name)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (Example name)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (Example name)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (Example name)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (Example name)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (Example name)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (Example name)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
        ];

        // Act
        var result = service.GenerateCommits(model, format);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GenerateCommits_StartDateToken_ShouldHaveValidMessages()
    {
        // Arrange
        var service = new CommitsService();
        var model = GetSampleValidPreset();
        var format = "Test ({start_date})";
        List<CommitModel> expected = [
            new ("Test (2023-01-01)", new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-01)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-01)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-01)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-01)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-01)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-01)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-01)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-01)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-01)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
        ];

        // Act
        var result = service.GenerateCommits(model, format);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GenerateCommits_CurrentDateToken_ShouldHaveValidMessages()
    {
        // Arrange
        var service = new CommitsService();
        var model = GetSampleValidPreset();
        var format = "Test ({current_date})";
        List<CommitModel> expected = [
            new ("Test (2023-01-01)", new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-03)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-03)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-05)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-05)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-05)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-06)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-06)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-06)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2023-01-06)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
        ];

        // Act
        var result = service.GenerateCommits(model, format);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GenerateCommits_CurrentDayToken_ShouldHaveValidMessages()
    {
        // Arrange
        var service = new CommitsService();
        var model = GetSampleValidPreset();
        var format = "Test ({current_day})";
        List<CommitModel> expected = [
            new ("Test (1)", new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (3)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (3)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (5)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (5)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (5)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (6)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (6)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (6)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (6)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
        ];

        // Act
        var result = service.GenerateCommits(model, format);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GenerateCommits_CurrentDayCommitToken_ShouldHaveValidMessages()
    {
        // Arrange
        var service = new CommitsService();
        var model = GetSampleValidPreset();
        var format = "Test ({current_day_commit})";
        List<CommitModel> expected = [
            new ("Test (1)", new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (1)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (1)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (3)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (1)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (3)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (4)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
        ];

        // Act
        var result = service.GenerateCommits(model, format);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GenerateCommits_CurrentTotalToken_ShouldHaveValidMessages()
    {
        // Arrange
        var service = new CommitsService();
        var model = GetSampleValidPreset();
        var format = "Test ({current_total})";
        List<CommitModel> expected = [
            new ("Test (1)", new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (2)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (3)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (4)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (5)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (6)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (7)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (8)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (9)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (10)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
        ];

        // Act
        var result = service.GenerateCommits(model, format);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GenerateCommits_TotalCountToken_ShouldHaveValidMessages()
    {
        // Arrange
        var service = new CommitsService();
        var model = GetSampleValidPreset();
        var format = "Test ({total_count})";
        List<CommitModel> expected = [
            new ("Test (10)", new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (10)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (10)", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (10)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (10)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (10)", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (10)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (10)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (10)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (10)", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
        ];

        // Act
        var result = service.GenerateCommits(model, format);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GenerateCommits_WhenUnknownToken_ShouldHaveValidMessages()
    {
        // Arrange
        var service = new CommitsService();
        var model = GetSampleValidPreset();
        var format = "Test (test} {nah123}";
        List<CommitModel> expected = [
            new ("Test (test} {nah123}", new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (test} {nah123}", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (test} {nah123}", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (test} {nah123}", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (test} {nah123}", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (test} {nah123}", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (test} {nah123}", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (test} {nah123}", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (test} {nah123}", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test (test} {nah123}", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
        ];

        // Act
        var result = service.GenerateCommits(model, format);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GenerateCommits_WhenNoTokens_ShouldHaveValidMessages()
    {
        // Arrange
        var service = new CommitsService();
        var model = GetSampleValidPreset();
        var format = "Test";
        List<CommitModel> expected = [
            new ("Test", new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test", new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test", new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
            new ("Test", new DateTimeOffset(2023, 1, 6, 0, 0, 0, TimeSpan.FromHours(12))),
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

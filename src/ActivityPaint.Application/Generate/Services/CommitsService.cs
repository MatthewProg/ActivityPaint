using ActivityPaint.Application.Abstractions.Repository.Models;
using ActivityPaint.Application.DTOs.Models;
using System.Text.RegularExpressions;

namespace ActivityPaint.Application.BusinessLogic.Generate.Services;

internal interface ICommitsService
{
    List<CommitModel> GenerateCommits(PresetModel model, string messageFormat);
}

internal partial class CommitsService : ICommitsService
{
    public List<CommitModel> GenerateCommits(PresetModel model, string messageFormat)
    {
        var metadata = GetMetadata(model);
        var output = new List<CommitModel>(metadata.TotalCommits);

        foreach (var commitsCount in model.CanvasData.Select(x => (int)x))
        {
            metadata.CurrentDay++;
            metadata.CurrentDayCommit = 0;

            while (metadata.CurrentDayCommit < commitsCount)
            {
                metadata.CurrentTotalCommit++;
                metadata.CurrentDayCommit++;

                var message = TokenRegex().Replace(messageFormat, (match) => GetTokenValue(metadata, match.Value));
                var date = metadata.StartDay.AddDays(metadata.CurrentDay - 1);

                output.Add(new(message, date));
            }
        }

        return output;
    }

    private static Metadata GetMetadata(PresetModel model) => new(
        Name: model.Name,
        StartDay: new DateTimeOffset(model.StartDate.Date, TimeSpan.FromHours(12)),
        CurrentDay: 0,
        CurrentDayCommit: 0,
        CurrentTotalCommit: 0,
        TotalCommits: model.CanvasData.Aggregate(0, (total, current) => total + (int)current)
    );

    private static string GetTokenValue(Metadata metadata, string token) => token[1..^1] switch
    {
        "name" => metadata.Name,
        "start_date" => metadata.StartDay.ToString("yyyy-MM-dd"),
        "current_date" => metadata.StartDay.AddDays(metadata.CurrentDay).ToString("yyyy-MM-dd"),
        "current_day" => metadata.CurrentDay.ToString(),
        "current_day_commit" => metadata.CurrentDayCommit.ToString(),
        "current_total" => metadata.CurrentTotalCommit.ToString(),
        "total_count" => metadata.TotalCommits.ToString(),
        _ => token
    };

    [GeneratedRegex(@"({\w+})+")]
    private static partial Regex TokenRegex();

    private record struct Metadata(
        string Name,
        DateTimeOffset StartDay,
        int CurrentDay,
        int CurrentDayCommit,
        int CurrentTotalCommit,
        int TotalCommits
    );
}

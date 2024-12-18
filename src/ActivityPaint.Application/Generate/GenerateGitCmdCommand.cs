using ActivityPaint.Application.BusinessLogic.Generate.Services;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Generate;

public sealed record GenerateGitCmdCommand(
    PresetModel Preset,
    string? MessageFormat = null
) : IResultRequest<string>;

internal class GenerateGitCmdCommandValidator : AbstractValidator<GenerateGitCmdCommand>
{
    public GenerateGitCmdCommandValidator(IEnumerable<IValidator<PresetModel>> presetValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);
    }
}

internal class GenerateGitCmdCommandHandler(ICommitsService commitsService) : IResultRequestHandler<GenerateGitCmdCommand, string>
{
    private readonly ICommitsService _commitsService = commitsService;

    public ValueTask<Result<string>> Handle(GenerateGitCmdCommand request, CancellationToken cancellationToken)
    {
        var commits = _commitsService.GenerateCommits(request.Preset, request.MessageFormat);

        if (commits.Count == 0)
        {
            return ValueTask.FromResult<Result<string>>(string.Empty);
        }

        var firstCommit = commits[0];
        var sizeApprox = GetCommand(firstCommit.DateTime, firstCommit.Message).Length * commits.Count;
        var builder = new StringBuilder(sizeApprox);

        for (var i = 0; i < commits.Count; i++)
        {
            var commit = commits[i];
            var message = GetCommand(commit.DateTime, commit.Message);

            if (i == commits.Count - 1)
            {
                message = message[..^1];
            }

            builder.Append(message);
        }

        return ValueTask.FromResult<Result<string>>(builder.ToString());
    }

    private static string GetCommand(DateTimeOffset date, string message)
        => $"git commit --allow-empty --no-verify --date={date:O} -m \"{message}\";\n";
}

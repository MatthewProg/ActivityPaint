using ActivityPaint.Application.BusinessLogic.Generate.Services;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Generate;

public record GenerateCommitsCommand(
    PresetModel Preset,
    string? MessageFormat
) : IResultRequest<List<CommitModel>>;

internal class GenerateCommitsCommandValidator : AbstractValidator<GenerateCommitsCommand>
{
    public GenerateCommitsCommandValidator(IEnumerable<IValidator<PresetModel>> presetValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);
    }
}

internal class GenerateCommitsCommandHandler : IResultRequestHandler<GenerateCommitsCommand, List<CommitModel>>
{
    private readonly ICommitsService _commitsService;

    public GenerateCommitsCommandHandler(ICommitsService commitsService)
    {
        _commitsService = commitsService;
    }

    public ValueTask<Result<List<CommitModel>>> Handle(GenerateCommitsCommand request, CancellationToken cancellationToken)
    {
        var commits = _commitsService.GenerateCommits(request.Preset, request.MessageFormat);

        return ValueTask.FromResult<Result<List<CommitModel>>>(commits);
    }
}

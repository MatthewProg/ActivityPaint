using ActivityPaint.Application.Abstractions.Repository;
using ActivityPaint.Application.BusinessLogic.Generate.Services;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Progress;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Generate;

public record GenerateRepoCommand(
    PresetModel Preset,
    AuthorModel Author,
    string Path,
    Progress? ProgressCallback = null
) : IResultRequest;

internal class GenerateRepoCommandValidator : AbstractValidator<GenerateRepoCommand>
{
    public GenerateRepoCommandValidator(IEnumerable<IValidator<PresetModel>> presetValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);
    }
}

internal class GenerateRepoCommandHandler : IResultRequestHandler<GenerateRepoCommand>
{
    private readonly IRepositoryService _repositoryService;
    private readonly ICommitsService _commitsService;

    public GenerateRepoCommandHandler(IRepositoryService repositoryService, ICommitsService commitsService)
    {
        _repositoryService = repositoryService;
        _commitsService = commitsService;
    }

    public ValueTask<Result> Handle(GenerateRepoCommand request, CancellationToken cancellationToken)
    {
        var commits = _commitsService.GenerateCommits(request.Preset);

        var creationResult = _repositoryService.InitOrPopulateRepository(request.Path, request.Author, commits, request.ProgressCallback);

        return ValueTask.FromResult(creationResult);
    }
}
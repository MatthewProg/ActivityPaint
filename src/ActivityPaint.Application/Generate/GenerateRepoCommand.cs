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

public sealed record GenerateRepoCommand(
    PresetModel Preset,
    AuthorModel Author,
    string Path,
    string? MessageFormat = null,
    Progress? ProgressCallback = null
) : IResultRequest;

internal class GenerateRepoCommandValidator : AbstractValidator<GenerateRepoCommand>
{
    public GenerateRepoCommandValidator(
        IEnumerable<IValidator<PresetModel>> presetValidators,
        IEnumerable<IValidator<AuthorModel>> authorValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);

        RuleFor(x => x.Author)
            .NotNull()
            .SetDefaultValidator(authorValidators);

        RuleFor(x => x.Path)
            .NotEmpty()
            .Path();
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
        var commits = _commitsService.GenerateCommits(request.Preset, request.MessageFormat);

        var creationResult = _repositoryService.InitOrPopulateRepository(request.Path, request.Author, commits, request.ProgressCallback);

        return ValueTask.FromResult(creationResult);
    }
}
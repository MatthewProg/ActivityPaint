using ActivityPaint.Application.Abstractions.Repository;
using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Generate.Services;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Progress;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;
using Mediator;

namespace ActivityPaint.Application.BusinessLogic.Generate;

public sealed record GenerateRepoCommand(
    PresetModel Preset,
    AuthorModel Author,
    bool Zip,
    string? Path = null,
    bool Overwrite = false,
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
            .Path();
    }
}

internal class GenerateRepoCommandHandler(
    IRepositoryService repositoryService,
    ICommitsService commitsService,
    IMediator mediator)
    : IResultRequestHandler<GenerateRepoCommand>
{
    private readonly IRepositoryService _repositoryService = repositoryService;
    private readonly ICommitsService _commitsService = commitsService;
    private readonly IMediator _mediator = mediator;

    public async ValueTask<Result> Handle(GenerateRepoCommand request, CancellationToken cancellationToken)
    {
        var commits = _commitsService.GenerateCommits(request.Preset, request.MessageFormat);

        if (!request.Zip)
        {
            if (string.IsNullOrWhiteSpace(request.Path))
            {
                return new Error(nameof(request.Path), "Save path must be provided when generating the repository");
            }

            var creationResult = _repositoryService.InitOrPopulateRepository(request.Path, request.Author, commits, request.ProgressCallback);
            return creationResult;
        }

        var streamResult = _repositoryService.CreateRepositoryZip(request.Author, commits, request.ProgressCallback);
        if (streamResult.IsFailure)
        {
            return streamResult.Error;
        }

        using var stream = streamResult.Value!;
        var saveCommand = new SaveToFileCommand(stream, $"{request.Preset.Name}.zip", request.Path, request.Overwrite);

        return await _mediator.Send(saveCommand, cancellationToken);
    }
}
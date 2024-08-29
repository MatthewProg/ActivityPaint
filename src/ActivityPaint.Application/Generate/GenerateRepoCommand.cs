using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Application.Abstractions.Interactions;
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

internal class GenerateRepoCommandHandler : IResultRequestHandler<GenerateRepoCommand>
{
    private readonly IFileSystemInteraction _fileSystemInteraction;
    private readonly IRepositoryService _repositoryService;
    private readonly IFileSaveService _fileSaveService;
    private readonly ICommitsService _commitsService;

    public GenerateRepoCommandHandler(IFileSystemInteraction fileSystemInteraction, IRepositoryService repositoryService,
                                      IFileSaveService fileSaveService, ICommitsService commitsService)
    {
        _fileSystemInteraction = fileSystemInteraction;
        _repositoryService = repositoryService;
        _fileSaveService = fileSaveService;
        _commitsService = commitsService;
    }

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

        if (string.IsNullOrWhiteSpace(request.Path))
        {
            var fileName = GetFileName(request.Preset.Name);

            return await _fileSystemInteraction.PromptFileSaveAsync(fileName, streamResult.Value!, cancellationToken);
        }

        return await _fileSaveService.SaveFileAsync(request.Path, streamResult.Value!, request.Overwrite, cancellationToken);
    }

    private static string GetFileName(string name)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitizedName = string.Join('_', name.Split(invalidChars));

        return $"{sanitizedName}.zip";
    }
}
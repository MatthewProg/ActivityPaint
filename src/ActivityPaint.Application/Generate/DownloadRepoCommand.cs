﻿using ActivityPaint.Application.Abstractions.Repository;
using ActivityPaint.Application.BusinessLogic.Generate.Services;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Progress;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Generate;

public sealed record DownloadRepoCommand(
    PresetModel Preset,
    AuthorModel Author,
    string? MessageFormat = null,
    Progress? ProgressCallback = null
) : IResultRequest<Stream>;

internal class DownloadRepoCommandValidator : AbstractValidator<DownloadRepoCommand>
{
    public DownloadRepoCommandValidator(
        IEnumerable<IValidator<PresetModel>> presetValidators,
        IEnumerable<IValidator<AuthorModel>> authorValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);

        RuleFor(x => x.Author)
            .NotNull()
            .SetDefaultValidator(authorValidators);
    }
}

internal class DownloadRepoCommandHandler : IResultRequestHandler<DownloadRepoCommand, Stream>
{
    private readonly IRepositoryService _repositoryService;
    private readonly ICommitsService _commitsService;

    public DownloadRepoCommandHandler(IRepositoryService repositoryService, ICommitsService commitsService)
    {
        _repositoryService = repositoryService;
        _commitsService = commitsService;
    }

    public ValueTask<Result<Stream>> Handle(DownloadRepoCommand request, CancellationToken cancellationToken)
    {
        var commits = _commitsService.GenerateCommits(request.Preset, request.MessageFormat);

        var streamResult = _repositoryService.CreateRepositoryZip(request.Author, commits, request.ProgressCallback);

        return ValueTask.FromResult(streamResult);
    }
}
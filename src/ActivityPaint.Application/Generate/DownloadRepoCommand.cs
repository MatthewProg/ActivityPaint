using ActivityPaint.Application.Abstractions.Repository;
using ActivityPaint.Application.Abstractions.Repository.Models;
using ActivityPaint.Application.BusinessLogic.Generate.Services;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Extensions;
using ActivityPaint.Application.DTOs.Models;
using ActivityPaint.Core.Shared.Progress;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Generate;

public record DownloadRepoCommand(
    PresetModel Preset,
    AuthorModel Author,
    Progress? ProgressCallback = null
) : IResultRequest<Stream>;

internal class DownloadRepoCommandValidator : AbstractValidator<DownloadRepoCommand>
{
    public DownloadRepoCommandValidator(IEnumerable<IValidator<PresetModel>> presetValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);
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
        var commits = _commitsService.GenerateCommits(request.Preset);

        var streamResult = _repositoryService.CreateRepositoryZip(request.Author, commits, request.ProgressCallback);

        return ValueTask.FromResult(streamResult);
    }
}
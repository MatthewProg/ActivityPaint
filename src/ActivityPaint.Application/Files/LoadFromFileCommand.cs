using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Files;

public sealed record LoadFromFileCommand(
    string? Path = null
) : IResultRequest<Stream>;

internal class LoadFromFileCommandValidator : AbstractValidator<LoadFromFileCommand>
{
    public LoadFromFileCommandValidator()
    {
        RuleFor(x => x.Path)
            .Path();
    }
}

internal class LoadFromFileCommandHandler(IFileSystemInteraction fileSystemInteraction, IFileLoadService fileLoadService) : IResultRequestHandler<LoadFromFileCommand, Stream>
{
    private readonly IFileSystemInteraction _fileSystemInteraction = fileSystemInteraction;
    private readonly IFileLoadService _fileLoadService = fileLoadService;

    public async ValueTask<Result<Stream>> Handle(LoadFromFileCommand request, CancellationToken cancellationToken)
    {
        return string.IsNullOrWhiteSpace(request.Path)
            ? await _fileSystemInteraction.PromptFileLoadAsync(cancellationToken)
            : _fileLoadService.GetFileStream(request.Path);
    }
}

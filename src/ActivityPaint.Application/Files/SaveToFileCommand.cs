using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Files;

public sealed record SaveToFileCommand(
    Stream DataStream,
    string SuggestedFileName,
    string? Path = null,
    bool Overwrite = false
) : IResultRequest;

internal class SaveToFileCommandValidator : AbstractValidator<SaveToFileCommand>
{
    public SaveToFileCommandValidator()
    {
        RuleFor(x => x.DataStream)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(x => x.CanRead).WithMessage("Stream must support reading.");

        RuleFor(x => x.SuggestedFileName)
            .NotEmpty();

        RuleFor(x => x.Path)
        .Path();
    }
}

internal class SaveToFileCommandHandler(IFileSystemInteraction fileSystemInteraction, IFileSaveService fileSaveService) : IResultRequestHandler<SaveToFileCommand>
{
    private readonly IFileSystemInteraction _fileSystemInteraction = fileSystemInteraction;
    private readonly IFileSaveService _fileSaveService = fileSaveService;

    public async ValueTask<Result> Handle(SaveToFileCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Path))
        {
            return await _fileSystemInteraction.PromptFileSaveAsync(request.SuggestedFileName, request.DataStream, cancellationToken);
        }

        return await _fileSaveService.SaveFileAsync(request.Path, request.DataStream, request.Overwrite, cancellationToken);
    }
}
using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;
using Mediator;

namespace ActivityPaint.Application.BusinessLogic.Preset;

public sealed record LoadPresetCommand(
    string? Path = null
) : IResultRequest<PresetModel?>;

internal class LoadPresetCommandValidator : AbstractValidator<LoadPresetCommand>
{
    public LoadPresetCommandValidator()
    {
        RuleFor(x => x.Path)
            .Path();
    }
}

internal class LoadPresetCommandHandler(IFileSystemInteraction fileSystemInteraction, IFileLoadService fileLoadService, IMediator mediator)
    : IResultRequestHandler<LoadPresetCommand, PresetModel?>
{
    private readonly IFileSystemInteraction _fileSystemInteraction = fileSystemInteraction;
    private readonly IFileLoadService _fileLoadService = fileLoadService;
    private readonly IMediator _mediator = mediator;

    public async ValueTask<Result<PresetModel?>> Handle(LoadPresetCommand command, CancellationToken cancellationToken)
    {
        var streamResult = string.IsNullOrWhiteSpace(command.Path)
            ? await _fileSystemInteraction.PromptFileLoadAsync(cancellationToken)
            : _fileLoadService.GetFileStream(command.Path);

        if (streamResult.IsFailure)
        {
            return streamResult.Error;
        }

        using var stream = streamResult.Value!;
        var parseCommand = new ParsePresetCommand(stream);

        return await _mediator.Send(parseCommand, cancellationToken);
    }
}
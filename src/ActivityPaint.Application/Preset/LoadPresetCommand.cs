using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Models;
using ActivityPaint.Core.Shared.Result;
using Mediator;

namespace ActivityPaint.Application.BusinessLogic.Preset;

public record LoadPresetCommand(
    string? Path = null
) : IResultRequest<PresetModel?>;

internal class LoadPresetCommandHandler : IResultRequestHandler<LoadPresetCommand, PresetModel?>
{
    private readonly IFileSystemInteraction _fileSystemInteraction;
    private readonly IFileLoadService _fileLoadService;
    private readonly IMediator _mediator;

    public LoadPresetCommandHandler(IFileSystemInteraction fileSystemInteraction, IFileLoadService fileLoadService, IMediator mediator)
    {
        _fileSystemInteraction = fileSystemInteraction;
        _fileLoadService = fileLoadService;
        _mediator = mediator;
    }

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
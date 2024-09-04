using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Files;

public sealed record SaveTextToFileCommand(
    string Text,
    string? Path = null,
    bool Overwrite = false
) : IResultRequest;

internal class SaveTextToFileCommandValidator : AbstractValidator<SaveTextToFileCommand>
{
    public SaveTextToFileCommandValidator()
    {
        RuleFor(x => x.Text)
            .NotNull();

        RuleFor(x => x.Path)
            .Path();
    }
}

internal class SaveTextToFileCommandHandler : IResultRequestHandler<SaveTextToFileCommand>
{
    private readonly IFileSystemInteraction _fileSystemInteraction;
    private readonly IFileSaveService _fileSaveService;

    public SaveTextToFileCommandHandler(IFileSystemInteraction fileSystemInteraction, IFileSaveService fileSaveService)
    {
        _fileSystemInteraction = fileSystemInteraction;
        _fileSaveService = fileSaveService;
    }

    public async ValueTask<Result> Handle(SaveTextToFileCommand command, CancellationToken cancellationToken)
    {
        using var data = new MemoryStream();
        using var writer = new StreamWriter(data, Encoding.UTF8);

        await writer.WriteAsync(command.Text);
        await writer.FlushAsync(cancellationToken);

        data.Seek(0, SeekOrigin.Begin);

        if (string.IsNullOrWhiteSpace(command.Path))
        {
            return await _fileSystemInteraction.PromptFileSaveAsync("save.txt", data, cancellationToken);
        }

        return await _fileSaveService.SaveFileAsync(command.Path, data, command.Overwrite, cancellationToken);
    }
}

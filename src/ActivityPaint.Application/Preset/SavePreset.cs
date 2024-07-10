using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Application.Abstractions.Services;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Core.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;
using System.Text.Json;

namespace ActivityPaint.Application.BusinessLogic.Preset;

public record SavePresetCommand(
    Core.Entities.Preset Preset,
    string? Path = null
) : IResultRequest;

internal class SavePresetCommandValidator : AbstractValidator<SavePresetCommand>
{
    public SavePresetCommandValidator(IEnumerable<IValidator<Core.Entities.Preset>> presetValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);
    }
}

internal class SavePresetCommandHandler : IResultRequestHandler<SavePresetCommand>
{
    private readonly IFileSystemInteraction _fileSystemInteraction;
    private readonly IFileSaveService _fileSaveService;

    public SavePresetCommandHandler(IFileSystemInteraction fileSystemInteraction, IFileSaveService fileSaveService)
    {
        _fileSystemInteraction = fileSystemInteraction;
        _fileSaveService = fileSaveService;
    }

    public async ValueTask<Result> Handle(SavePresetCommand command, CancellationToken cancellationToken)
    {
        using var data = new MemoryStream();

        await JsonSerializer.SerializeAsync(data, command.Preset, cancellationToken: cancellationToken);
        data.Seek(0, SeekOrigin.Begin);

        if (string.IsNullOrWhiteSpace(command.Path))
        {
            var fileName = GetFileName(command.Preset.Name);
            return await _fileSystemInteraction.PromptFileSaveAsync(fileName, data);
        }

        return await _fileSaveService.SaveFileAsync(command.Path, data);
    }

    private static string GetFileName(string name)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanetizedName = string.Join('_', name.Split(invalidChars));

        return $"{sanetizedName}.json";
    }
}
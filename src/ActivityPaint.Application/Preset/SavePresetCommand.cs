using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Application.BusinessLogic.Preset.Mappers;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;
using System.Text.Json;

namespace ActivityPaint.Application.BusinessLogic.Preset;

public record SavePresetCommand(
    PresetModel Preset,
    string? Path = null,
    bool Overwrite = false
) : IResultRequest;

internal class SavePresetCommandValidator : AbstractValidator<SavePresetCommand>
{
    public SavePresetCommandValidator(IEnumerable<IValidator<PresetModel>> presetValidators)
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

        var model = command.Preset.ToPresetFileModel();
        await JsonSerializer.SerializeAsync(data, model, cancellationToken: cancellationToken);
        data.Seek(0, SeekOrigin.Begin);

        if (string.IsNullOrWhiteSpace(command.Path))
        {
            var fileName = GetFileName(model.Name);
            return await _fileSystemInteraction.PromptFileSaveAsync(fileName, data, cancellationToken);
        }

        return await _fileSaveService.SaveFileAsync(command.Path, data, command.Overwrite, cancellationToken);
    }

    private static string GetFileName(string name)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitizedName = string.Join('_', name.Split(invalidChars));

        return $"{sanitizedName}.json";
    }
}
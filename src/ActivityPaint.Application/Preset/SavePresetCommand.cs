using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Preset.Mappers;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;
using Mediator;
using System.Text.Json;

namespace ActivityPaint.Application.BusinessLogic.Preset;

public sealed record SavePresetCommand(
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

        RuleFor(x => x.Path)
            .Path();
    }
}

internal class SavePresetCommandHandler(IMediator mediator) : IResultRequestHandler<SavePresetCommand>
{
    private readonly IMediator _mediator = mediator;

    public async ValueTask<Result> Handle(SavePresetCommand command, CancellationToken cancellationToken)
    {
        using var data = new MemoryStream();

        var model = command.Preset.ToPresetFileModel();
        await JsonSerializer.SerializeAsync(data, model, cancellationToken: cancellationToken);
        data.Seek(0, SeekOrigin.Begin);

        var saveCommand = new SaveToFileCommand(data, $"{model.Name}.json", command.Path, command.Overwrite);

        return await _mediator.Send(saveCommand, cancellationToken);
    }
}
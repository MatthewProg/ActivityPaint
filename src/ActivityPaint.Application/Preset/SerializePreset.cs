using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Core.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Preset;

public record SerializePresetCommand(Core.Entities.Preset Preset)
    : IResultRequest<string>;

internal class SerializePresetCommandValidator : AbstractValidator<SerializePresetCommand>
{
    public SerializePresetCommandValidator(IEnumerable<IValidator<Core.Entities.Preset>> presetValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);
    }
}

internal class SerializePresetCommandHandler : IResultRequestHandler<SerializePresetCommand, string>
{
    public ValueTask<Result<string>> Handle(SerializePresetCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Preset;

public record SerializePresetCommand(Core.Entities.Preset Preset)
    : IResultRequest<string>;

internal class SerializePresetCommandValidator : AbstractValidator<SerializePresetCommand>
{
    public SerializePresetCommandValidator()
    {
        RuleFor(x => x.Preset)
            .NotNull();
    }
}

internal class SerializePresetCommandHandler : IResultRequestHandler<SerializePresetCommand, string>
{
    public ValueTask<Result<string>> Handle(SerializePresetCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
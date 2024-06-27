using ActivityPaint.Application.Shared.Mediator;
using ActivityPaint.Core.Models;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.Preset;

public record SerializePresetCommand(PresetModel Preset)
    : IResultRequest<string>;

internal class SerializePresetCommandHandler : IResultRequestHandler<SerializePresetCommand, string>
{
    public ValueTask<Result<string>> Handle(SerializePresetCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
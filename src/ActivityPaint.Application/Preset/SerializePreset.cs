using ActivityPaint.Application.Shared;
using ActivityPaint.Core.Models;
using ActivityPaint.Core.Shared.Result;
using System.Text;

namespace ActivityPaint.Application.Preset;

public record SerializePresetCommand(PresetModel Preset)
    : IResultCommand<string>;

internal class SerializePresetCommandHandler : IResultCommandHandler<SerializePresetCommand, string>
{
    public ValueTask<Result<string>> Handle(SerializePresetCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
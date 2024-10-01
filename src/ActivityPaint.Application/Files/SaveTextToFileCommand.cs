using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;
using Mediator;
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

internal class SaveTextToFileCommandHandler(IMediator mediator) : IResultRequestHandler<SaveTextToFileCommand>
{
    private readonly IMediator _mediator = mediator;

    public async ValueTask<Result> Handle(SaveTextToFileCommand command, CancellationToken cancellationToken)
    {
        using var data = new MemoryStream();
        using var writer = new StreamWriter(data, Encoding.UTF8);

        await writer.WriteAsync(command.Text);
        await writer.FlushAsync(cancellationToken);

        data.Seek(0, SeekOrigin.Begin);

        var saveCommand = new SaveToFileCommand(data, "save.txt", command.Path, command.Overwrite);
        return await _mediator.Send(saveCommand, cancellationToken);
    }
}

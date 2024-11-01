using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.BusinessLogic.Gallery;

public record DeleteGalleryItemCommand(
    int Id
) : IResultRequest;

internal class DeleteGalleryItemCommandHandler(IPresetRepository presetRepository) : IResultRequestHandler<DeleteGalleryItemCommand>
{
    private readonly IPresetRepository _presetRepository = presetRepository;

    public async ValueTask<Result> Handle(DeleteGalleryItemCommand request, CancellationToken cancellationToken)
    {
        await _presetRepository.DeleteAsync(request.Id, cancellationToken);

        return Result.Success();
    }
}

using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.BusinessLogic.Gallery;

public record GetGalleryItemsCountCommand()
    : IResultRequest<int>;

internal class GetGalleryItemsCountCommandHandler(IPresetRepository presetRepository) : IResultRequestHandler<GetGalleryItemsCountCommand, int>
{
    private readonly IPresetRepository _presetRepository = presetRepository;

    public async ValueTask<Result<int>> Handle(GetGalleryItemsCountCommand request, CancellationToken cancellationToken)
    {
        return await _presetRepository.GetCount(cancellationToken);
    }
}

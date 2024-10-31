using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Gallery;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.BusinessLogic.Gallery;

public record LoadGalleryItemsCommand(
    int Page,
    int PageSize = 18
) : IResultRequest<IEnumerable<GalleryModel>>;

internal class LoadGalleryItemsCommandHandler(IPresetRepository presetRepository) : IResultRequestHandler<LoadGalleryItemsCommand, IEnumerable<GalleryModel>>
{
    private readonly IPresetRepository _presetRepository = presetRepository;

    public async ValueTask<Result<IEnumerable<GalleryModel>>> Handle(LoadGalleryItemsCommand request, CancellationToken cancellationToken)
    {
        var page = await _presetRepository.GetPageAsync(request.Page, request.PageSize, cancellationToken);

        var result = page.Select(x => x.ToGalleryModel());

        return Result<IEnumerable<GalleryModel>>.Success(result);
    }
}

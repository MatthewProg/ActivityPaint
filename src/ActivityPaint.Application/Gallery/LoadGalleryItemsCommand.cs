using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.BusinessLogic.Gallery;

public record LoadGalleryItemsCommand(
    int Page,
    int PageSize = 18
) : IResultRequest<IEnumerable<PresetModel>>;

internal class LoadGalleryItemsCommandHandler(IPresetRepository presetRepository) : IResultRequestHandler<LoadGalleryItemsCommand, IEnumerable<PresetModel>>
{
    private readonly IPresetRepository _presetRepository = presetRepository;

    public async ValueTask<Result<IEnumerable<PresetModel>>> Handle(LoadGalleryItemsCommand request, CancellationToken cancellationToken)
    {
        var page = await _presetRepository.GetPageAsync(request.Page, request.PageSize, cancellationToken);

        var result = page.Select(x => x.ToPresetModel());

        return Result<IEnumerable<PresetModel>>.Success(result);
    }
}

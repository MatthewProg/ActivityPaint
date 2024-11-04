using Riok.Mapperly.Abstractions;
using PresetEntity = ActivityPaint.Core.Entities.Preset;

namespace ActivityPaint.Application.DTOs.Gallery;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GalleryModelMap
{
    public static partial GalleryModel ToGalleryModel(this PresetEntity preset);

    public static partial PresetEntity ToPreset(this GalleryModel galleryModel);
}

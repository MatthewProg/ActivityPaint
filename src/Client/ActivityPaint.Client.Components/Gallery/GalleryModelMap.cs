using ActivityPaint.Application.DTOs.Gallery;
using ActivityPaint.Client.Components.Editor;

namespace ActivityPaint.Client.Components.Gallery;

public static class GalleryModelMap
{
    public static EditorModel ToEditorModel(this GalleryModel galleryModel) => new()
    {
        IsDarkModeDefault = galleryModel.IsDarkModeDefault,
        CanvasData = galleryModel.CanvasData,
        StartDate = galleryModel.StartDate,
        Name = galleryModel.Name
    };
}

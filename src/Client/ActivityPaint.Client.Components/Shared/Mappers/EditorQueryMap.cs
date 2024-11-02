using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Client.Components.Editor;
using ActivityPaint.Core.Helpers;
using System.Collections.Specialized;
using System.Globalization;

namespace ActivityPaint.Client.Components.Shared.Mappers;

public static class EditorQueryMap
{
    private const string PARAM_NAME = "name";
    private const string PARAM_START_DATE = "startDate";
    private const string PARAM_DATA = "data";
    private const string PARAM_DARK_MODE = "darkMode";

    private const string START_DATE_FORMAT = "yyyy-MM-dd";

    public static EditorModel QueryToEditor(NameValueCollection query)
    {
        var name = query.Get(PARAM_NAME);
        var dateString = query.Get(PARAM_START_DATE);
        var dataString = query.Get(PARAM_DATA);
        var darkModeString = query.Get(PARAM_DARK_MODE);

        return new()
        {
            Name = name,
            StartDate = DateTime.TryParseExact(dateString, START_DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : null,
            CanvasData = dataString is null ? null : CanvasDataHelper.ConvertToList(dataString),
            IsDarkModeDefault = bool.TryParse(darkModeString, out var darkMode) ? darkMode : null
        };
    }

    public static Dictionary<string, object?> PresetToQuery(PresetModel preset) => new()
    {
        [PARAM_NAME] = preset.Name,
        [PARAM_START_DATE] = preset.StartDate.ToString(START_DATE_FORMAT),
        [PARAM_DATA] = preset.CanvasDataString,
        [PARAM_DARK_MODE] = preset.IsDarkModeDefault.ToString()
    };
}
